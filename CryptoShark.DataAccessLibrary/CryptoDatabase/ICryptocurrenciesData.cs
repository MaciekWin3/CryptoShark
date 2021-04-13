using CryptoShark.DataAccessLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoShark.DataAccessLibrary.CryptoDatabase
{
    public interface ICryptocurrenciesData
    {
        Task<List<CryptocurrencySqlModel>> GetAllCryptoRecords();
        Task<List<CryptocurrencySqlModel>> GetLastCryptoRecords();
        Task InsertCryptoData(CryptocurrencySqlModel crypto);
    }
}