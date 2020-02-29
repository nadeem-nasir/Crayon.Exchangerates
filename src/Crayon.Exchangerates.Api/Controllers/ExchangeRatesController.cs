using Crayon.Exchangerates.Api.Helpers;
using Crayon.Exchangerates.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace Crayon.Exchangerates.Api.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("api/[controller]")]
    public class ExchangeRatesController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IExchangeRatesHelper _helper;
        public ExchangeRatesController(IHttpClientFactory clientFactory, IExchangeRatesHelper helper)
        {
            _clientFactory = clientFactory;
            _helper = helper;
        }

        /// <summary>
        /// get exchange rate from api and return the result
        /// </summary>
        /// <param name="queryInputModel"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ExchangeRatesResponseModel), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetExchangeRates([FromQuery] ExchangeRatesQueryInputModel queryInputModel)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(x => x.Errors));
            }
            var inputModel = _helper.Mapper(queryInputModel);
            if (inputModel.ExchangeRatesDates.Count < 2)
            {
                ModelState.AddModelError(nameof(inputModel.ExchangeRatesDates), "ExchangeRatesDates must have at least two date values");
                return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(x => x.Errors));
            }

            inputModel = _helper.OrderInputDates(inputModel);

            var endPointUrl = _helper.GetEndPointUrl("history", inputModel.BaseCurrency, inputModel.TargetCurrency, inputModel.ExchangeRatesDates.First().ExchangeRatesDate, inputModel.ExchangeRatesDates.Last().ExchangeRatesDate);
            var client = _clientFactory.CreateClient("ExchangeRates");
            var apiResponse = await client.GetAsync(endPointUrl);
            if (apiResponse.IsSuccessStatusCode)
            {
                var responseString = await apiResponse.Content.ReadAsStringAsync();
                if (responseString.Length <= 0)
                {
                    return StatusCode(StatusCodes.Status204NoContent);
                }
                var exchangeRatesHistoryResult = _helper.ParseExchangeRatesHistory(responseString, inputModel);
                var response = _helper.CreateExchangeRatesResponse(exchangeRatesHistoryResult);
                return StatusCode(StatusCodes.Status200OK, response);
            } 
            return StatusCode((int)apiResponse.StatusCode, apiResponse.ReasonPhrase);
        }
    }
}
