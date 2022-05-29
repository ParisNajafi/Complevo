using Core.Dtos.User;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ITokenService
    {
        Task<bool> ValidateToken(string token);
        Task<Tokens> Authenticate(LoginUserModel user);
    }

}
