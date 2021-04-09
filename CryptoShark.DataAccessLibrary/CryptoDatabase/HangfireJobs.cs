using CryptoShark.DataAccessLibrary.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;



namespace CryptoShark.DataAccessLibrary.CryptoDatabase
{
    public class HangfireJobs
    {

        public static HttpClient ApiClient { get; set; }

        private readonly string[] CryptoList = new string[10]
        { "btc-usd", "eth-usd", "bnb-usd", "ada-usd", "dot-usd", "link-usd", "xmr-usd", "dash-usd", "zil-usd", "rvn-usd"};


        public void CallApiAndSave()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            CryptocurrencyModel CryptoModel = new CryptocurrencyModel();
            CryptocurrencySqlModel CryptoSqlModel = new CryptocurrencySqlModel();
            string nameOfCryptoCurrency;
            string nameOfCurrency;
            double price;
            double volume;
            double change;


            using (var data = new WebClient())
            {
                foreach (string crypto in CryptoList)
                {
                    try
                    {
                        string response = data.DownloadString("https://api.cryptonator.com/api/ticker/" + crypto);
                        CryptoModel = JsonConvert.DeserializeObject<CryptocurrencyModel>(response);


                        
                        nameOfCryptoCurrency = CryptoModel.Ticker.Base;
                        nameOfCurrency = CryptoModel.Ticker.Target;
                        price = double.Parse(CryptoModel.Ticker.Price);
                        volume = double.Parse(CryptoModel.Ticker.Volume);
                        change = double.Parse(CryptoModel.Ticker.Change);
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }


                    Console.WriteLine(price);

                }
                
            }


        }

        public static void xd()
        {
            Console.WriteLine("działa");
        }

    }
}
