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
            string name;
            double 


            using (var data = new WebClient())
            {
                foreach (string crypto in CryptoList)
                {
                    try
                    {
                        string response = data.DownloadString("https://api.cryptonator.com/api/ticker/" + crypto);
                        CryptoModel = JsonConvert.DeserializeObject<CryptocurrencyModel>(response);
                        price = CryptoModel.Ticker.Price;
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
