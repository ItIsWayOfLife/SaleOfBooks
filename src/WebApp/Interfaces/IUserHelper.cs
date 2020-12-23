
namespace WebApp.Interfaces
{
    internal interface IUserHelper
    {
        string GetIdUserById(string id);
        string GetIdUserByEmail(string email);
    }
}
