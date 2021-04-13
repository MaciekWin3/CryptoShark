using CryptoShark.DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using CryptoShark.DataAccessLibrary.CryptoDatabase.DataAccessLibrary;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;

namespace CryptoShark.DataAccessLibrary.CryptoDatabase
{
    public class CryptocurrenciesData : ICryptocurrenciesData
    {
        private readonly ISqlDataAccess _db;

        public CryptocurrenciesData(ISqlDataAccess db)
        {
            _db = db;
        }

        public static HttpClient ApiClient { get; set; }


        //{ "btc-usd", "eth-usd", "bnb-usd", "ada-usd", "dot-usd", "link-usd", "xmr-usd", "dash-usd", "zil-usd", "rvn-usd"};


        public Task<List<CryptocurrencySqlModel>> GetAllCryptoRecords()
        {
            string sql = "select * from dbo.Cryptocurrencies";

            return _db.LoadData<CryptocurrencySqlModel, dynamic>(sql, new { });
        }

        public Task<List<CryptocurrencySqlModel>> GetLastCryptoRecords() //do poprawy, rozwiązanie tymczasowe
        {
            string sql = @"/****** Script for SelectTopNRows command from SSMS  ******/
                        SELECT TOP (10) [id]
                              ,[base]
                              ,[currency]
                              ,[price]
                              ,[volume]
                              ,[change]
                              ,[timestamp]
                              ,[datetime]
                          FROM [CryptoShark].[dbo].[Cryptocurrencies]
                          ORDER BY id DESC";

            return _db.LoadData<CryptocurrencySqlModel, dynamic>(sql, new { });
        }

        public Task InsertCryptoData(CryptocurrencySqlModel crypto)
        {

            DateTimeOffset date = DateTime.Now;

            string sql = @"insert into dbo.Crytpocurrencies
                         (base, currency, price, volume, change, dateofcallepoch, dateofcall)
                         values (@Base, @Target, @Price, @Volume, @Change, @Timestamp, @date);";

            return _db.SaveData(sql, crypto);
        }
    }
}

