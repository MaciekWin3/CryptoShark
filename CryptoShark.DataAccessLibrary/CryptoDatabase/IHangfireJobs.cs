using CryptoShark.DataAccessLibrary.Models;

namespace CryptoShark.DataAccessLibrary.CryptoDatabase
{
    public interface IHangfireJobs
    {
        UserDataModel Find(string Email);
    }
}