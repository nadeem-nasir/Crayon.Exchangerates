using System.ComponentModel.DataAnnotations;

namespace Crayon.Exchangerates.Api.Models
{
    public class ExchangeRatesQueryInputModel
    {
        /// <summary>
        /// Gets or sets base currency
        /// </summary>
        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string BaseCurrency { get; set; }

        /// <summary>
        /// Gets or sets target currency
        /// </summary>
        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string TargetCurrency { get; set; }

        /// <summary>
        /// Gets or sets comma seperated list of dates
        /// </summary>
        [Required]
        [MinLength(20)]
        public string ExchangeRatesDates { get; set; }

    }
}
