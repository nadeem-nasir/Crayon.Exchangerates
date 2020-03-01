using Crayon.Exchangerates.Api.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Crayon.Exchangerates.Api.Helpers
{

    public class ExchangeRatesHelper : IExchangeRatesHelper
    {

        /// <summary>
        /// Format url according to api
        /// </summary>
        /// <param name="url"></param>
        /// <param name="baseCurrency"></param>
        /// <param name="targetCurrency"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public string GetEndPointUrl(string url, string baseCurrency, string targetCurrency, DateTime? startDate, DateTime? endDate)
        {
            return $"{url}?start_at={startDate.Value.ToString("yyyy-MM-dd")}&end_at={endDate.Value.ToString("yyyy-MM-dd")}&symbols={baseCurrency},{targetCurrency}";
        }

        /// <summary>
        /// Order according to ExchangeRatesDates
        /// </summary>
        /// <param name="inputModel"></param>
        /// <returns></returns>
        public ExchangeRatesInputModel OrderInputDates(ExchangeRatesInputModel inputModel)
        {
            inputModel.ExchangeRatesDates = inputModel.ExchangeRatesDates.OrderBy(d => d.ExchangeRatesDate).ToList();
            return inputModel;
        }

        /// <summary>
        /// Parse the api json and return the model
        /// </summary>
        /// <param name="exchangeRatesHistoryData"></param>
        /// <param name="inputModel"></param>
        /// <returns></returns>
        public List<ExchangeRatesHistoryModel> ParseExchangeRatesHistory(string exchangeRatesHistoryData, ExchangeRatesInputModel inputModel)
        {
            var exchangeRatesHistory = JObject.Parse(exchangeRatesHistoryData).Value<JObject>("rates");

            List<ExchangeRatesHistoryModel> exchangeRatesHistoryResult = new List<ExchangeRatesHistoryModel>();
            foreach (var rateHistory in exchangeRatesHistory)
            {
                if (rateHistory.Value.HasValues)
                {
                    var exchangeRatesDate = DateTime.Parse(rateHistory.Key);
                    if (inputModel.ExchangeRatesDates.Any(d => d.ExchangeRatesDate == exchangeRatesDate))
                    {
                        exchangeRatesHistoryResult.Add(new ExchangeRatesHistoryModel
                        {
                            ExchangeRatesDate = exchangeRatesDate,
                            BaseCurrenyRate = rateHistory.Value[inputModel.BaseCurrency].Value<float>(),
                            TargetCurrenyRate = rateHistory.Value[inputModel.TargetCurrency].Value<float>()
                        });
                    }
                }
            }
            return exchangeRatesHistoryResult;
        }

        /// <summary>
        /// Create response from model
        /// </summary>
        /// <param name="exchangeRatesHistoryModels"></param>
        /// <returns></returns>
        public ExchangeRatesResponseModel CreateExchangeRatesResponse(List<ExchangeRatesHistoryModel> exchangeRatesHistoryModels)
        {
            exchangeRatesHistoryModels = exchangeRatesHistoryModels.OrderBy(r => r.Rate).ToList();
            var min = exchangeRatesHistoryModels.First();
            var max = exchangeRatesHistoryModels.Last();
            var avg = exchangeRatesHistoryModels.Average(av => av.Rate);
            return new ExchangeRatesResponseModel
            {
                MinRate = $"A min rate of {min.Rate} on {min.ExchangeRatesDate.ToString("yyyy-MM-dd")}",
                MaxRate = $"A max rate of {max.Rate} on {max.ExchangeRatesDate.ToString("yyyy-MM-dd")}",
                AvgRate = $"An average rate of {avg}"
            };
        }
        /// <summary>
        /// Map and convert comma seprated values into dates
        /// </summary>
        /// <param name="fromModel"></param>
        /// <returns></returns>
        public ExchangeRatesInputModel Mapper(ExchangeRatesQueryInputModel fromModel)
        {
            var toModel = new ExchangeRatesInputModel
            {
                BaseCurrency = fromModel.BaseCurrency,
                TargetCurrency = fromModel.TargetCurrency
            };
            var splitExchangeRatesDates = fromModel.ExchangeRatesDates.Split(",");
            foreach (var ratesDate in splitExchangeRatesDates)
            {
                if (DateTime.TryParse(ratesDate, out DateTime ratesDateResult))
                {
                    toModel.ExchangeRatesDates.Add(new ExchangeRatesDatesInputModel
                    {
                        ExchangeRatesDate = ratesDateResult
                    });
                }
            }
            return toModel;
        }
    }
}
