using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using User_API.Entities;
using User_API.Services;

namespace UserAPI.Controller
{

    [Route("api/[controller]")]
    [ApiController]
    public class PostLogin(IUserService userService) : ControllerBase
    {
        private readonly IUserService userService = userService;
       
        [HttpGet]
        [Route("GetUser")]
        public IActionResult GetUsers(int? id = null)
        {
            var users = userService.GetAllUsers(id);
            if (id == null) return Ok(users);

            return Ok(users);
        }


        [HttpPut]
        [Route("UpdateUserDetails")]
        public IActionResult UpdateUser(string? userName, string? newName, string? newCountry)
        {
            var newUpdatedUser = userService.UpdateUser(userName, newName, newCountry);

            return Ok(newUpdatedUser);
        }

        [HttpDelete]
        [Route("PermanentDelete")]
        public IActionResult DeleteUser(int id)
        {
            var deletedUser = userService.DeleteUser(id);

            return Ok(deletedUser);
        }

        [HttpDelete]
        [Route("SoftDelete")]
        public IActionResult SoftDeleteUser(int id)
        {
            var softDeletedUser = userService.SoftDeleteUser(id);
            return Ok(softDeletedUser);
        }

        [HttpPost]
        [Route("ChangePassword")]
        public IActionResult ChangePassword(string userName, string oldPassword, string newPassword)
        {
            var checkStatus = userService.ChangePassword(userName, oldPassword, newPassword);
            return Ok(checkStatus);
        }
    }
}