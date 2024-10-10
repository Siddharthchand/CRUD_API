using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using User_API.ExceptionHandlerMiddleware;

namespace User_API.Services
{
    public class TokenGeneratorService(IConfiguration configuration)
    {
        private readonly IConfiguration configuration = configuration;

        public CommonResponse TokenGenerator(string userName)
        {
            //Creating claim for token payload.

            var claims = new[] { new Claim("userName", userName) };

            var getKeyFromAppSettings = configuration["Jwt:key"] ?? throw new CustomException(ErrorCode.InvalidToken,null);

            // key has to be in bytes as the hash func needs i/p in  bytes.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(getKeyFromAppSettings)); 

            // creating a signature with the help of the above key and a hashing algo:HmacSha256
            var signature = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //creating a new token with signature and payload
            var token = new JwtSecurityToken(claims: claims, issuer: null,             /* or, configuration["Jwt:Issuer"]*/
                                            audience: null,                           /*or, configuration["Jwt:Audience"]*/
                                            expires: DateTime.UtcNow.AddDays(1),
                                            signingCredentials: signature
            );

            //Correction: add your own function to check expiry date for the token and send an error message.
            string tokenValue = new JwtSecurityTokenHandler().WriteToken(token); // converting a token to string

            return new CommonResponse(ErrorCode.Success,tokenValue);
        }
    }
}
