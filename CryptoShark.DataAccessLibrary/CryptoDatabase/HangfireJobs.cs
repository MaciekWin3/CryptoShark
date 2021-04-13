using CryptoShark.DataAccessLibrary.Models;
using CryptoShark.DataAccessLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using CryptoShark.DataAccessLibrary.CryptoDatabase.DataAccessLibrary;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using System.Threading.Tasks;
using System.Data.SqlTypes;
using System.Globalization;

namespace CryptoShark.DataAccessLibrary.CryptoDatabase
{
    public class HangfireJobs
    {
        public static void CallApiAndSave(string connectionString)
        {
            string[] CryptoList = new string[10]
            { "btc-usd", "eth-usd", "bnb-usd", "ada-usd", "dot-usd", "link-usd", "xmr-usd", "dash-usd", "zil-usd", "rvn-usd"};

            Console.ForegroundColor = ConsoleColor.Red;
           
            DateTime myDateTime = DateTime.Now;
            string sqlFormattedDate = myDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");

            
            CryptocurrencyModel CryptoModel;
            CryptocurrencySqlModel CryptoSqlModel = new CryptocurrencySqlModel();

            foreach (string element in CryptoList)
            {
                using (var data = new WebClient())
                {
                    string response = data.DownloadString("https://api.cryptonator.com/api/ticker/" + element);
                    CryptoModel = JsonConvert.DeserializeObject<CryptocurrencyModel>(response);

                    CryptoSqlModel.Base = CryptoModel.Ticker.Base;
                    CryptoSqlModel.Target = CryptoModel.Ticker.Target;
                    CryptoSqlModel.Price = Double.Parse(CryptoModel.Ticker.Price, CultureInfo.InvariantCulture);
                    CryptoSqlModel.Volume = Double.Parse(CryptoModel.Ticker.Volume, CultureInfo.InvariantCulture);
                    CryptoSqlModel.Change = Double.Parse(CryptoModel.Ticker.Change, CultureInfo.InvariantCulture);
                    CryptoSqlModel.Timestamp = CryptoModel.Timestamp;
                    CryptoSqlModel.Date = sqlFormattedDate;

                    using (IDbConnection db = new SqlConnection(connectionString))
                    {
                        string sql = @"insert into [dbo].[Cryptocurrencies]
                                ([base], [currency], [price], [volume], [change], [timestamp], [datetime] )
                                values (@Base, @Target, @Price, @Volume, @Change, @Timestamp, @date);";

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
                }
            }
        }
    }
}
