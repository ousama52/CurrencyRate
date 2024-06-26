using System.ComponentModel.DataAnnotations;

namespace MyProject.Controllers.Model
{
    public class CurrencyDto
    {
        [Required]
        public required string CurrencyN { get; set; }

        [Required]
        public decimal Exchangerate { get; set; }
    }
}
