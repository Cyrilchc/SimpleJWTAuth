using System.Threading.Tasks;
using DBAccessLibrary.Models;

namespace DBAccessLibrary.Queries
{
    public interface IUserData
    {
        Task<DataUserModel> GetUserByMail(string mail);
        Task<bool> CheckMailExists(string mail);
        Task InserUser(DataUserModel userModel);
    }
}