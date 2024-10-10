using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using User_API.Entities;
using User_API.ExceptionHandlerMiddleware;
using User_API.Services;

namespace User_API.Controller
{

    [Route("api/[controller]")]
    [ApiController]
    public class PreLogin(IClientService ClientService, TokenGeneratorService tokenGenerator) : ControllerBase
    {
        private readonly IClientService ClientService = ClientService;
        private readonly TokenGeneratorService tokenGenerator = tokenGenerator;

        [HttpPost]
        [Route("Register")]
        public IActionResult RegisterUser(AddUser addUser)
        {
            var newUser = ClientService.RegisterUser(addUser);
            return Ok(newUser);
        }

        [HttpGet]
        [Route("Login")]
        public IActionResult UserLogin(string userName,string Password)
        {

            ClientService.LoginService(userName, Password);
            var token = tokenGenerator.TokenGenerator(userName);
            return Ok(token);
        }

    }
}
