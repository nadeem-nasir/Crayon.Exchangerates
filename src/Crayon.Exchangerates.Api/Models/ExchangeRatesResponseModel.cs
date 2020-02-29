namespace Crayon.Exchangerates.Api.Models
{
    public class ExchangeRatesResponseModel
    {
        /// <summary>
        /// Gets or sets min rate
        /// </summary>
        public string MinRate { get; set; }
        /// <summary>
        /// Gets or sets exchange max rate
        /// </summary>
        public string MaxRate { get; set; }
        /// <summary>
        /// Gets or sets exchange avg rate
        /// </summary>
        public string AvgRate { get; set; }
    }
}
