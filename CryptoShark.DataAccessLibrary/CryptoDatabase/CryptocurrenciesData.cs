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
using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Identity;


namespace CryptoShark.DataAccessLibrary.CryptoDatabase
{
    public class CryptocurrenciesData : ICryptocurrenciesData
    {
        private readonly ISqlDataAccess _db;



        public CryptocurrenciesData(ISqlDataAccess db)
        {
            _db = db;
        }


        //{ "btc-usd", "eth-usd", "bnb-usd", "ada-usd", "dot-usd", "link-usd", "xmr-usd", "dash-usd", "zil-usd", "rvn-usd"};


        public Task<List<CryptocurrencySqlModel>> GetAllCryptoRecords()
        {
            string sql = @"select * from dbo.Cryptocurrencies";

            return _db.LoadData<CryptocurrencySqlModel, dynamic>(sql, new { });
        }

        public Task<List<UserDataModel>> GetAllUsers(string name)
        {
            string sql = "select * from dbo.UserData";


            var dane = _db.LoadData<UserDataModel, dynamic>(sql, new { });
            if(dane == null)
            {
                Console.WriteLine("tu nikogo nie ma!");
            }
            else
            {
                
                Console.WriteLine(dane.ToString());
            }

            return _db.LoadData<UserDataModel, dynamic>(sql, new { });
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
                          FROM [CryptoSharkAuth].[dbo].[Cryptocurrencies]
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

        public static async Task CreateUserData(string id, string email)
        {

            await Task.Delay(1000);

            DateTime myDateTime = DateTime.Now;

            UserDataModel newUser = new UserDataModel();

            newUser.UserId = id;
            newUser.Email = email;
            newUser.Bitcoin = 0;
            newUser.Ethereum = 0;
            newUser.BinanceCoin = 0;
            newUser.Polkadot = 0;
            newUser.Chainlink = 0;
            newUser.Monero = 0;
            newUser.Dash = 0;
            newUser.Zilliqa = 0;
            newUser.RavenCoin = 0;
            newUser.EtherumClassic = 0;
            newUser.Date = myDateTime;

            using (IDbConnection db = new SqlConnection("Data Source=.\\sqlexpress;Initial Catalog=CryptoSharkAuth;Integrated Security=True;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                string sql = @"insert into [dbo].[UserData]
                                ([userId], [email], [bitcoin], [ethereum], [binancecoin], [polkadot], [chainlink],[monero],
                                [dash], [zilliqa], [ravencoin], [etherumclassic], [save])
                                values (@UserId, @Email, @Bitcoin, @Ethereum, @BinanceCoin, @Polkadot, @Chainlink, @Monero,
                                @Dash, @Zilliqa, @RavenCoin, @EtherumClassic, @Date);";

                try
                {
                    var result = db.Execute(sql, new
                    {
                        newUser.UserId,
                        newUser.Email,
                        newUser.Bitcoin,
                        newUser.Ethereum,
                        newUser.BinanceCoin,
                        newUser.Polkadot,
                        newUser.Chainlink,
                        newUser.Monero,
                        newUser.Dash,
                        newUser.Zilliqa,
                        newUser.RavenCoin,
                        newUser.EtherumClassic,
                        newUser.Date
                    });
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error with {0}", ex.Message);
                }

            }

        }


    }
}

