using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;

namespace CryptoShark.DataAccessLibrary.Models
{
    public class CryptocurrencySqlModel
    {
        public long Id { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string Nameid { get; set; }
        public int Rank { get; set; }
        public double Price_usd { get; set; }
        public double Percent_change_24h { get; set; }
        public double Percent_change_1h { get; set; }
        public double Percent_change_7d { get; set; }
        public string Market_cap_usd { get; set; }
        public string Volume24 { get; set; }
        public string Volume24_native { get; set; }
        public string Csupply { get; set; }
        public string Price_btc { get; set; }
        public string Tsupply { get; set; }
        public string Msupply { get; set; }
        public DateTime Date { get; set; }
    }
}
