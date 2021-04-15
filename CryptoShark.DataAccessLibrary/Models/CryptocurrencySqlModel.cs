using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;

namespace CryptoShark.DataAccessLibrary.Models
{
    public class CryptocurrencySqlModel
    {
        public long Id { get; set; }

        //name of cryptocurrency
        public string Base { get; set; }
        //name of currency
        public string Target { get; set; }
        public double Price { get; set; }
        public double Volume { get; set; }
        public double Change { get; set; }
        public int Timestamp { get; set; }
        public DateTime Date { get; set; }
    }
}
