using Core.Identity;

namespace WebApp.Interfaces
{
    public interface IUserHelper
    {
        string GetIdUserById(string id);
        string GetIdUserByEmail(string email);

        bool CheckUserExists(string id);
        ApplicationUser GetUserById(string id);
    }
}
