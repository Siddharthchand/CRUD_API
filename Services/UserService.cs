using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using User_API.Entities;
using User_API.Data;
using User_API.DataAccessObject;
using User_API.ExceptionHandlerMiddleware;
using User_API.Enum;
using User_API.PasswordHandler;
namespace User_API.Services
{
    public interface IUserService
    {
        CommonResponse GetAllUsers(int? Id);
        CommonResponse UpdateUser(string? userName, string? newName, string? newCountry);
        CommonResponse DeleteUser(int id);
        CommonResponse SoftDeleteUser(int id);
        public CommonResponse ChangePassword(string userName, string newPassword, string oldPassword);
    }

    public class UserServiceClass(IClientDao clientdao, IUserDao userdao, PasswordHasher256 passwordHandler) : IUserService
    {
        private readonly IUserDao userdao = userdao;
        private readonly IClientDao clientdao = clientdao;
        private readonly PasswordHasher256 passwordHandler = passwordHandler; // using primary constructor

        public CommonResponse GetAllUsers(int? Id = null)// optimize with Or/null condition
        {
            if (Id == null)
            {
                var allUsers = userdao.GetAllUsersDao(Id);
                if (allUsers == null || allUsers.Count == 0) throw new CustomException(ErrorCode.NotFound,null);
                return new CommonResponse(ErrorCode.Success,allUsers);
            }

            var userId = userdao.GetAllUsersDao(Id);/* dbContext.Users.Find(Id);*/// other methods
            if (userId == null || userId[0] == null) throw new CustomException(ErrorCode.NotFound,null);
            return new CommonResponse(ErrorCode.Success,userId);
        }

        public CommonResponse UpdateUser(string? userName, string? newName, string? newCountry)
        {
            if (userName == null || newName == null || newCountry == null || newName.Length < 3 || newName.Length > 20
                || newCountry.Length < 3 || newCountry.Length > 20)

                throw new CustomException(ErrorCode.Unauthorised,null);

            var user = userdao.FindUser(userName: userName); // FindUserBy username in dao class

            if (user == null || ((int)user.Status) == -1)
                throw new CustomException(ErrorCode.TerminatedUser, null);

            var findName = userdao.FindUser(Name: newName);// check if name is already taken

            if (findName != null) throw new CustomException(ErrorCode.AlreadyExists, null); // optimize

            if (!string.IsNullOrEmpty(newName))
                user.Name = newName;
            if (!string.IsNullOrEmpty(newCountry))
                user.Country = newCountry;

            userdao.UpdateUserDao(user);

            return new CommonResponse(ErrorCode.Success,user);
        }
        public CommonResponse DeleteUser(int id) //permanent delete //terminate
        {
            var user = userdao.FindUser(id) ?? throw new CustomException(ErrorCode.NotFound, null);
            user.Status = UserStatus.terminate;
            userdao.DeleteUserDao(user);
            return new CommonResponse(ErrorCode.Success,user);
        }

        public CommonResponse SoftDeleteUser(int id) //inactive
        {
            var user = userdao.FindUser(id) ?? throw new CustomException(ErrorCode.NotFound, null);
            user.Status = UserStatus.Inactive;
            userdao.SoftDeleteUserDao(user);
            return new CommonResponse(ErrorCode.Success,user);
        }


        public CommonResponse ChangePassword(string userName, string oldPassword, string newPassword)
        {
            var oldPasswordHash = passwordHandler.HashPassword(oldPassword);
            var user = userdao.FindUser(userName: userName);

            var client = clientdao.FindClient(userName: userName);

            if (user == null || client == null || client.Password != oldPasswordHash || ((int)user.Status) == -1)
                throw new CustomException(ErrorCode.FormatWrong, null);

            var newPasswordHash = passwordHandler.HashPassword(newPassword);
            client.Password = newPasswordHash;

            clientdao.ChangePassword(client);

            return new CommonResponse(ErrorCode.Success,client);
        }
    }
}
