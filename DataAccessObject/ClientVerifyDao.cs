using User_API.Data;
using User_API.Entities;
namespace User_API.DataAccessObject
{
    public interface IClientVerifyDao
    {
        public VerifyClient? VerifyClientDao(string? ClientId);
    }

    public class ClientVerifyClass(AppdbContext context) : IClientVerifyDao
    {
        private readonly AppdbContext context = context;

        public VerifyClient? VerifyClientDao(string? ClientId)
        {
            return context.VerifyClients.Find(ClientId);
        }

    }
}
