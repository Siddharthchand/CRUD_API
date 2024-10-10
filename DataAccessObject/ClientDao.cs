using Microsoft.EntityFrameworkCore;
using User_API.Data;
using User_API.Entities;

namespace User_API.DataAccessObject
{
    public interface IClientDao
    {
        public void RegisterUserDao(User newUser,Client client);

        public void ChangePassword(Client client);
        public Client? FindClient(int? Id=null,string? userName=null);


    }

    public class ClientDaoClass(AppdbContext context) : IClientDao
    {
        private readonly AppdbContext context = context;

        public void RegisterUserDao(User newUser,Client newClient)
        {
           
            context.Clients.Add(newClient);

            context.Users.Add(newUser);

            context.SaveChanges();

        }

        //public bool LoginUserDao(string userName)
        //{
        //    var client=context.Clients.FirstOrDefault(x => x.UserName == userName);
        //    if (client == null) return false;
        //    return true;

        //}

        public Client? FindClient(int? Id=null, string? userName=null)
        {
            if (Id != null) return context.Clients.Find(Id); //  Find is used to find primary key value

            if (userName != null) return context.Clients.FirstOrDefault(x => x.UserName == userName);

            return null;
        }

        public void ChangePassword(Client client)
        {
            context.Clients.Update(client);
            context.SaveChanges();
        }
      
    }
}
