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
            //{ "btc-usd", "eth-usd", "bnb-usd", "etc-usd", "dot-usd", "link-usd", "xmr-usd", "dash-usd", "zil-usd", "rvn-usd", uniswap};   //etc118        

            int[] CryptoList = new int[10] { 90, 80, 2710, 45219, 2751, 28, 8, 32334, 32386, 47305 };
            //int[] CryptoList = new int[1] { 90 };

            DateTime myDateTime = DateTime.Now;
            CoinLoreModel CryptoModel;
            Crypto[] CryptoData = new Crypto[1];
            CryptocurrencySqlModel CryptoSqlModel = new CryptocurrencySqlModel();

            foreach (int element in CryptoList)
            {
                await Task.Delay(1000);
                using (var data = new WebClient())
                {
                    try
                    {
                        string response = data.DownloadString("https://api.coinlore.net/api/ticker/?id=" + element);
                        CryptoData = JsonConvert.DeserializeObject<Crypto[]>(response);
                    }
                    catch (WebException e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    CryptoSqlModel.Symbol = CryptoData[0].Symbol;
                    CryptoSqlModel.Name = CryptoData[0].Name;
                    CryptoSqlModel.Nameid = CryptoData[0].Nameid;
                    CryptoSqlModel.Rank = CryptoData[0].Rank;
                    CryptoSqlModel.Market_cap_usd = CryptoData[0].Market_cap_usd;
                    CryptoSqlModel.Volume24 = CryptoData[0].Volume24;
                    CryptoSqlModel.Volume24_native = CryptoData[0].Volume24_native;
                    CryptoSqlModel.Csupply = CryptoData[0].Csupply;
                    CryptoSqlModel.Price_btc = CryptoData[0].Price_btc;
                    CryptoSqlModel.Tsupply = CryptoData[0].Tsupply;
                    CryptoSqlModel.Msupply = CryptoData[0].Msupply;

                    try
                    {
                        CryptoSqlModel.Price_usd = Double.Parse(CryptoData[0].Price_usd, CultureInfo.InvariantCulture);
                        CryptoSqlModel.Percent_change_24h = Double.Parse(CryptoData[0].Percent_change_24h, CultureInfo.InvariantCulture);
                        CryptoSqlModel.Percent_change_1h = Double.Parse(CryptoData[0].Percent_change_1h, CultureInfo.InvariantCulture);
                        CryptoSqlModel.Percent_change_7d = Double.Parse(CryptoData[0].Percent_change_7d, CultureInfo.InvariantCulture);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error parsing data to database!");
                    }

                    CryptoSqlModel.Date = myDateTime;
                    Console.WriteLine("skolowano");

                    using (IDbConnection db = new SqlConnection(connectionString))
                    {
                        string sql = @"insert into [dbo].[CryptocurrenciesCoinLore]
                                ([symbol], [name], [nameid], [rank], [price_usd], [percent_change_24h], [percent_change_1h], [percent_change_7d],
                                [market_cap_usd], [volume24], [volume24_native], [csupply], [price_btc], [tsupply], [msupply], [datetime])
                                values (@Symbol, @Name, @Nameid, @Rank, @Price_usd, @Percent_change_24h, @Percent_change_1h,
                                @Percent_change_7d, @Market_cap_usd, @Volume24, @Volume24_native, @Csupply, @Price_btc, @Tsupply, @Msupply, @Date );";

                        try
                        {
                            var result = db.Execute(sql, new
                            {

                                CryptoSqlModel.Symbol,
                                CryptoSqlModel.Name,
                                CryptoSqlModel.Nameid,
                                CryptoSqlModel.Rank,
                                CryptoSqlModel.Price_usd,
                                CryptoSqlModel.Percent_change_24h,
                                CryptoSqlModel.Percent_change_1h,
                                CryptoSqlModel.Percent_change_7d,
                                CryptoSqlModel.Market_cap_usd,
                                CryptoSqlModel.Volume24,
                                CryptoSqlModel.Volume24_native,
                                CryptoSqlModel.Csupply,
                                CryptoSqlModel.Price_btc,
                                CryptoSqlModel.Tsupply,
                                CryptoSqlModel.Msupply,
                                CryptoSqlModel.Date
                                
                            });
                            Console.WriteLine("Robie cos");
                        }
                        catch (Exception ex)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Error with {0}", CryptoSqlModel.Name);
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
                                [dash], [zilliqa], [ravencoin], [uniswap], [save])
                                select distinct [userId], [email], [bitcoin], [ethereum], [binancecoin], [polkadot], [chainlink],[monero],
                                [dash], [zilliqa], [ravencoin], [uniswap], getdate() from UserData;";

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
            newUser.Uniswap = 0;
            newUser.Date = myDateTime;

            using (IDbConnection db = new SqlConnection("Data Source=.\\sqlexpress;Initial Catalog=CryptoSharkAuth;Integrated Security=True;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                string sql = @"insert into [dbo].[UserData]
                                ([userId], [email], [bitcoin], [ethereum], [binancecoin], [polkadot], [chainlink],[monero],
                                [dash], [zilliqa], [ravencoin], [uniswap], [save])
                                values (@UserId, @Email, @Bitcoin, @Ethereum, @BinanceCoin, @Polkadot, @Chainlink, @Monero,
                                @Dash, @Zilliqa, @RavenCoin, @Uniswap, @Date);";

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
                        newUser.Uniswap,
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
