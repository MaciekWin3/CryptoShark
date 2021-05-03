using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CryptoShark.BlazorServer.Models
{
    public class AmountModel
    {
        public double Amount { get; set; }
        public bool Operation { get; set; }
    }
}
