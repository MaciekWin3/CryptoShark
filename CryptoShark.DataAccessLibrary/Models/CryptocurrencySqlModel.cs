using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoShark.DataAccessLibrary.Models
{
    public class CryptocurrencySqlModel
    {
        public string Base { get; set; }
        public string Target { get; set; }
        public string Price { get; set; }
        public string Volume { get; set; }
        public string Change { get; set; }
        public ulong Timestamp { get; set; }
        public DateTime Date { get; set; }
    }
}
