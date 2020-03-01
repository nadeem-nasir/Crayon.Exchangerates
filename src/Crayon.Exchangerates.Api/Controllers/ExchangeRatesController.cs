using Crayon.Exchangerates.Api.Helpers;
using Crayon.Exchangerates.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
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
            try
            {
                //Validate model
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(x => x.Errors));
                }
               //Map from query string values and split the values
                var inputModel = _helper.Mapper(queryInputModel);
                //validate if date input are correct 
                if (inputModel.ExchangeRatesDates.Count < 2)
                {
                    ModelState.AddModelError(nameof(inputModel.ExchangeRatesDates), "ExchangeRatesDates must have at least two date values");
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState.Values.SelectMany(x => x.Errors));
                }
                
                //Order the input into asc order 
                inputModel = _helper.OrderInputDates(inputModel);

                //Get the end point
                var endPointUrl = _helper.GetEndPointUrl("history", inputModel.BaseCurrency, inputModel.TargetCurrency, inputModel.ExchangeRatesDates.First().ExchangeRatesDate, inputModel.ExchangeRatesDates.Last().ExchangeRatesDate);
                //Create http client
                var client = _clientFactory.CreateClient("ExchangeRates");
               //Make a request and get response
                var apiResponse = await client.GetAsync(endPointUrl);
               //Validate the response 
                if (apiResponse.IsSuccessStatusCode)
                {
                    var responseString = await apiResponse.Content.ReadAsStringAsync();
                    //If empty response then return no content
                    if (responseString.Length <= 0)
                    {
                        return StatusCode(StatusCodes.Status204NoContent);
                    }
                    //Parse the json api response into class model
                    var exchangeRatesHistoryResult = _helper.ParseExchangeRatesHistory(responseString, inputModel);
                   //Create the response that we want to return 
                    var response = _helper.CreateExchangeRatesResponse(exchangeRatesHistoryResult);
                   //return the response 
                    return StatusCode(StatusCodes.Status200OK, response);
                }
                //If not valid response , return the error
                return StatusCode((int)apiResponse.StatusCode, apiResponse.ReasonPhrase);
            }
            catch(Exception ex )
            {
                //return if any exception
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
