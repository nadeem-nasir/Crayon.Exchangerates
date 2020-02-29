using Crayon.Exchangerates.Api.Models;
using System;
using System.Collections.Generic;

namespace Crayon.Exchangerates.Api.Helpers
{
    public interface IExchangeRatesHelper
    {
        /// <summary>
        /// get the api endpoint url 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="baseCurrency"></param>
        /// <param name="targetCurrency"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        string GetEndPointUrl(string url, string baseCurrency, string targetCurrency, DateTime? startDate, DateTime? endDate);
        /// <summary>
        /// order input date in asc order 
        /// </summary>
        /// <param name="inputModel"></param>
        /// <returns></returns>
        ExchangeRatesInputModel OrderInputDates(ExchangeRatesInputModel inputModel);
        /// <summary>
        /// parse string json api data in to class model
        /// </summary>
        /// <param name="exchangeRatesHistoryData"></param>
        /// <param name="inputModel"></param>
        /// <returns></returns>
        List<ExchangeRatesHistoryModel> ParseExchangeRatesHistory(string exchangeRatesHistoryData, ExchangeRatesInputModel inputModel);
        /// <summary>
        /// Create the response from ExchangeRatesHistoryModel
        /// </summary>
        /// <param name="exchangeRatesHistoryModels"></param>
        /// <returns></returns>
        ExchangeRatesResponseModel CreateExchangeRatesResponse(List<ExchangeRatesHistoryModel> exchangeRatesHistoryModels);
        /// <summary>
        /// Mapp query string input into ExchangeRatesInputModel
        /// </summary>
        /// <param name="fromModel"></param>
        /// <returns></returns>
        ExchangeRatesInputModel Mapper(ExchangeRatesQueryInputModel fromModel);
    }
}
