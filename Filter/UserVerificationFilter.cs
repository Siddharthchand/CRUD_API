using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using User_API.ExceptionHandlerMiddleware;
using User_API.PasswordHandler;
using User_API.DataAccessObject;
using System.Security.Claims;
using Azure.Core;
using User_API.Services;

namespace User_API.Filter;

public class UserVerificationFilter(IClientService clientservice,IConfiguration configuration) : IActionFilter
{
    private readonly IConfiguration configuration = configuration;
    private readonly IClientService clientservice=clientservice;

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var path = context.HttpContext.Request.Path.Value ?? 
            throw new CustomException(ErrorCode.NotFound,null);

        var requestHeaders = context.HttpContext.Request.Headers;
        if (path.ToLower().Contains("/prelogin", StringComparison.OrdinalIgnoreCase))
        {
            //verifying client ID and secret after extracting ffrom headers from context.
            var clientId= requestHeaders["ClientId"].ToString();
            var clientSecret= requestHeaders["ClientSecret"].ToString();

            if(clientId == null || clientSecret == null) 
                throw new CustomException(ErrorCode.NotFound,null);

            var validClient = clientservice.VerifyClient(clientId,clientSecret);

            if(!validClient) 
                throw new CustomException(ErrorCode.NotFound,null);
        }

        // for postlogin (validate token)
        else if (path.Contains("/postlogin", StringComparison.OrdinalIgnoreCase)) 
        {
            // extract the token from header and check if it's name is "token" as given in post-filter.
            if (!requestHeaders.TryGetValue("token", out var value)) 
            {
                context.Result = new BadRequestObjectResult("Missing Token");
                return;
            }
            var token = value.ToString();
            
            if (!ValidateToken(token))
            {
                throw new CustomException(ErrorCode.InvalidToken,null);
            }
        }        
    }

    public bool ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler(); // JwtSecurityTokenHandler class helps validate JWT tokens.

        var getKeyFromAppSettings = configuration["Jwt:key"] ?? throw new CustomException(ErrorCode.InvalidToken,null);
        var key = Encoding.UTF8.GetBytes(getKeyFromAppSettings); //convert key to bytes for cryptographic operations.

        var tokenParameters = new TokenValidationParameters //token
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
        // ValidateToken is a method in JwtSecurityTokenHandler used to verify tokens
        var tokenValidator =tokenHandler.ValidateToken(token, tokenParameters, validatedToken: out SecurityToken ValidatedToken);
        if (tokenValidator == null) return false;
        return true;
    }
    public void OnActionExecuted(ActionExecutedContext context) // it is invoked after anything on controller is executed.
    {
    }

}