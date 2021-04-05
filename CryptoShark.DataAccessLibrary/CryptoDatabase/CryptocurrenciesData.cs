using CryptoShark.CryptoDatabase.DataAccessLibrary;
using CryptoShark.DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace CryptoShark.DataAccessLibrary.CryptoDatabase
{
    public class CryptocurrenciesData : ICryptocurrenciesData
    {
        private readonly ISqlDataAccess _db;
        public CryptocurrenciesData(ISqlDataAccess db)
        {
            _db = db;
        }

        public Task<List<CryptocurrencySqlModel>> GetAllCryptoRecords()
        {
            string sql = "select * from dbo.Cryptocurrencies";

            return _db.LoadData<CryptocurrencySqlModel, dynamic>(sql, new { });
        }

        public Task InsertCrytpoData(CryptocurrencySqlModel crypto)
        {

            DateTimeOffset date = DateTime.Now;

            string sql = @"insert into dbo.Crytpocurrencies
                         (base, currency, price, volume, change, dateofcallepoch, dateofcall)
                         values (@Base, @Target, @Price, @Volume, @Change, @Timestamp, @date);";

            return _db.SaveData(sql, crypto);
        }
    }
}
