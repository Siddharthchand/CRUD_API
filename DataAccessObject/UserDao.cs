using User_API.Entities;
using User_API.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace User_API.DataAccessObject
{
    public interface IUserDao
    {
        public List<User>? GetAllUsersDao(int? Id);

        public User AddNewUser(User newUser);
        public User? FindUser(int? Id = null, string? Name = null,string? userName=null);
        public User UpdateUserDao(User user);

        public void DeleteUserDao(User user);
        
        public void SoftDeleteUserDao(User user);

    }

    public class UserDaoClass(AppdbContext context) : IUserDao
    {
        private readonly AppdbContext context = context;

        public User AddNewUser(User newUser)
        {
            context.Users.Add(newUser);
            context.SaveChanges();

            return newUser;
        }

        public List<User>? GetAllUsersDao(int? Id)
        {
            return Id == null ? [.. context.Users] : [context.Users.Find(Id)];
        }

        public User UpdateUserDao(User user)
        {
            context.Users.Update(user);
            context.SaveChanges();
            return user;
        }

        public void DeleteUserDao(User user)
        {
            context.Users.Update(user);
            context.SaveChanges();
        }

        public void SoftDeleteUserDao(User user)
        {
            context.Users.Update(user);
            context.SaveChanges();
        }

        public User? FindUser(int? Id = null, string? Name = null,string? userName=null)
        {
            if (Id != null) return context.Users.Find(Id);

            if (Name != null) return context.Users.FirstOrDefault(x => x.Name == Name);

            if (userName != null) return context.Users.FirstOrDefault(x => x.UserName == userName);


            return null;
        }

    }
}
