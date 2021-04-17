using CryptoShark.DataAccessLibrary.Models;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace CryptoShark.DataAccessLibrary.CryptoDatabase
{
    public class HangfireJobs : IHangfireJobs
    {

        private IDbConnection db;
        public HangfireJobs(IConfiguration configuration)
        {
            this.db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public HangfireJobs()
        {

        }

        public UserDataModel Find(string Email)
        {
            var sql = "SELECT * FROM UserData WHERE Email = @Email";
            return db.Query<UserDataModel>(sql, new { @Email = Email }).Single();
        }

        public static async Task CallApiAndSave(string connectionString)
        {
            //string[] CryptoList = new string[10]
            //{ "btc-usd", "eth-usd", "bnb-usd", "etc-usd", "dot-usd", "link-usd", "xmr-usd", "dash-usd", "zil-usd", "rvn-usd"};           

            string[] CryptoList = new string[3] { "btc-usd", "eth-usd", "bnb-usd" };

            DateTime myDateTime = DateTime.Now;
            CryptocurrencyModel CryptoModel;
            CryptocurrencySqlModel CryptoSqlModel = new CryptocurrencySqlModel();

            foreach (string element in CryptoList)
            {
                await Task.Delay(1000);
                using (var data = new WebClient())
                {
                    string response = data.DownloadString("https://api.cryptonator.com/api/ticker/" + element);
                    CryptoModel = JsonConvert.DeserializeObject<CryptocurrencyModel>(response);

                    CryptoSqlModel.Base = CryptoModel.Ticker.Base;
                    CryptoSqlModel.Target = CryptoModel.Ticker.Target;
                    try
                    {
                        CryptoSqlModel.Price = Double.Parse(CryptoModel.Ticker.Price, CultureInfo.InvariantCulture);
                        CryptoSqlModel.Volume = Double.Parse(CryptoModel.Ticker.Volume, CultureInfo.InvariantCulture);
                        CryptoSqlModel.Change = Double.Parse(CryptoModel.Ticker.Change, CultureInfo.InvariantCulture);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("poza skalą");
                    }
                    CryptoSqlModel.Timestamp = CryptoModel.Timestamp;
                    CryptoSqlModel.Date = myDateTime;

                    using (IDbConnection db = new SqlConnection(connectionString))
                    {
                        string sql = @"insert into [dbo].[Cryptocurrencies]
                                ([base], [currency], [price], [volume], [change], [timestamp], [datetime] )
                                values (@Base, @Target, @Price, @Volume, @Change, @Timestamp, @Date);";

                        try
                        {
                            var result = db.Execute(sql, new
                            {
                                CryptoSqlModel.Base,
                                CryptoSqlModel.Target,
                                CryptoSqlModel.Price,
                                CryptoSqlModel.Volume,
                                CryptoSqlModel.Change,
                                CryptoSqlModel.Timestamp,
                                CryptoSqlModel.Date
                            });
                        }
                        catch (Exception ex)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Error with {0}", CryptoSqlModel.Base);
                            Console.WriteLine(ex.Message);
                        }

                    }
                }
            }
        }



    }
}
