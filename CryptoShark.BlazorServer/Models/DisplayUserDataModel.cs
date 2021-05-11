using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoShark.BlazorServer.Models
{
    public class DisplayUserDataModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public double Bitcoin { get; set; }
        public double Ethereum { get; set; }
        public double BinanceCoin { get; set; }
        public double Polkadot { get; set; }
        public double Chainlink { get; set; }
        public double Monero { get; set; }
        public double Dash { get; set; }
        public double Zilliqa { get; set; }
        public double RavenCoin { get; set; }
        public double Uniswap { get; set; }
        public DateTime Date { get; set; }
    }
}
