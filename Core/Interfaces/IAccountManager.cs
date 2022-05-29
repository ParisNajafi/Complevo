using Core.Dtos.User;
using Core.Entities;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IAccountManager: IRepository<User>
    {
        Task RegisterUser(RegisterUserModel model);
        Task<bool> IsValidUser(string username, string password);
    }
}
