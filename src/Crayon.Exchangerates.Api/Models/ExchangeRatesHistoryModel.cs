using System;

namespace Crayon.Exchangerates.Api.Models
{
    public class ExchangeRatesHistoryModel
    {
        /// <summary>
        /// Gets or sets exchange rate date
        /// </summary>
        public DateTime ExchangeRatesDate { get; set; }

        /// <summary>
        /// Gets or sets the base currency rate
        /// </summary>
        public float BaseCurrenyRate { get; set; }

        /// <summary>
        /// Gets or sets target currency rate
        /// </summary>
        public float TargetCurrenyRate { get; set; }

        /// <summary>
        /// Get the Rate by TargetCurrenyRate / BaseCurrenyRate
        /// </summary>
        public float Rate
        {
            get
            {
                return TargetCurrenyRate / BaseCurrenyRate;
            }
        }
    }
}
