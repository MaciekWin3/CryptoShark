using System.ComponentModel.DataAnnotations;

namespace CryptoShark.BlazorServer.Models
{
    public class AmountModel
    {
        [Required(ErrorMessage ="{0} is required!")]
        [RegularExpression(@"^\d+\.\d{0,2}$")]
        [Range(0, 9999999.99)]
        public double Amount { get; set; }
        public bool Operation { get; set; }
    }
}
