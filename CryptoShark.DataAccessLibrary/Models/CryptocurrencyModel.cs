using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoShark.DataAccessLibrary.Models
{
    public class CryptocurrencyModel
    {
        public CryptocurrencyTicker Ticker { get; set; }
        public int Timestamp { get; set; }
    }
}
