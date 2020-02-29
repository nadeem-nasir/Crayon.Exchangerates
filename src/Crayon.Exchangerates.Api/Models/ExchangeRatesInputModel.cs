using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Crayon.Exchangerates.Api.Models
{
    
    public class ExchangeRatesInputModel
    {
        public ExchangeRatesInputModel()
        {
            ExchangeRatesDates = new List<ExchangeRatesDatesInputModel>();
        }
        /// <summary>
        ///  Gets or sets base currency
        /// </summary>
        [Required]
        public string BaseCurrency { get; set; }
        /// <summary>
        ///  Gets or sets target currency
        /// </summary>
        [Required]
        public string TargetCurrency { get; set; }
        /// <summary>
        ///  Gets or sets exchange rate input dates
        /// </summary>
        [Required]
        [MinLength(2)]
        public IList<ExchangeRatesDatesInputModel> ExchangeRatesDates { get; set; }
    }

    public class ExchangeRatesDatesInputModel
    {
        /// <summary>
        /// Gets or sets exchange rate input date
        /// </summary>
        [DataType(DataType.DateTime)]
        [Required]
        public DateTime? ExchangeRatesDate { get; set; }
    }
}
