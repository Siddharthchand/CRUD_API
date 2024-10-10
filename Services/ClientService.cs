using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using User_API.Data;
using User_API.DataAccessObject;
using User_API.Entities;
using User_API.Enum;
using User_API.ExceptionHandlerMiddleware;
using User_API.PasswordHandler;

namespace User_API.Services
{
    public interface IClientService
    {
        CommonResponse RegisterUser(AddUser addUser);
        
        public CommonResponse LoginService(string username,string password);

        public bool VerifyClient(string ClientId, string Clientsecret);
    }

    public class ClientServiceClass(IClientVerifyDao clientverifydao,IUserDao userdao,IClientDao clientdao, PasswordHasher256 passwordHandler) : IClientService
    {
        private readonly PasswordHasher256 passwordHandler = passwordHandler;
        private readonly IClientDao clientdao = clientdao;
        private readonly IUserDao userdao=userdao;
        private readonly IClientVerifyDao clientverifydao = clientverifydao;

        public CommonResponse RegisterUser(AddUser addUser)
        {
            if (string.IsNullOrEmpty(addUser.UserName) || string.IsNullOrEmpty(addUser.Country) || (addUser.Name.Length < 3 || addUser.Name.Length > 50) || (addUser.Country.Length < 3 || addUser.Country.Length > 50))
                throw new CustomException(ErrorCode.FormatWrong,null); // unique username
            string hashPassword = passwordHandler.HashPassword(addUser.Password);


            var userName = addUser.UserName;
            var newUser = new User
            {
                Name = addUser.Name,
                Country = addUser.Country,
                UserName = userName,
                Status=UserStatus.Active,
                CreatedAt = DateTime.Now.ToString("yyyy-MM-dd h:mm:ss tt"),
            };
            var newClient = new Client
            {
                UserName = userName,
                Password = hashPassword,
            };
            clientdao.RegisterUserDao(newUser, newClient);
            return new CommonResponse(ErrorCode.Success,newUser);
        }

        public CommonResponse LoginService(string userName, string Password)
        {
            //check if user is terminated
            var user = userdao.FindUser(userName: userName) ??
                throw new CustomException(ErrorCode.NotFound,null);

            if (((int)user.Status) == -1)
                throw new CustomException(ErrorCode.TerminatedUser,null);

            //get user's passsword if valid user

            var client = clientdao.FindClient(userName: userName);
            var getClientPassword = client?.Password??
                throw new CustomException(ErrorCode.Unauthorised,null);

            string hashedEnteredPassword = passwordHandler.HashPassword(Password);

            // comparing hash of both passwords
            bool verifyPassword = passwordHandler.verifyPasswords(hashedEnteredPassword, getClientPassword);

            if (verifyPassword == false)
                throw new CustomException(ErrorCode.Unauthorised,null);         

            return new CommonResponse(ErrorCode.Success,client);
        }

        public bool VerifyClient(string ClientId, string Clientsecret)
        {
            var client= clientverifydao.VerifyClientDao(ClientId);
            if(client == null || client.ClientSecret != Clientsecret) return false;

            return true;
        }
    }
}

