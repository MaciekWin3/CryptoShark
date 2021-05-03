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
using System.Collections.Generic;

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

        public void ReturnZero()
        {
            Console.WriteLine("0");
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

        public static async  Task SaveUserData(string connectionString)
        {
            await Task.Delay(100);
            using (IDbConnection db = new SqlConnection("Data Source=.\\sqlexpress;Initial Catalog=CryptoSharkAuth;Integrated Security=True;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                string sql = @"insert into [dbo].[UserData]
                                ([userId], [email], [bitcoin], [ethereum], [binancecoin], [polkadot], [chainlink],[monero],
                                [dash], [zilliqa], [ravencoin], [etherumclassic], [save])
                                select distinct [userId], [email], [bitcoin], [ethereum], [binancecoin], [polkadot], [chainlink],[monero],
                                [dash], [zilliqa], [ravencoin], [etherumclassic], getdate() from UserData;";

                db.Execute(sql);
                

            }

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
