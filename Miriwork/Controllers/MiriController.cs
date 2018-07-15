using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Miriwork.Contracts;

namespace Miriwork.Controllers
{
    /// <summary>
    /// Controller for internal use. Handles Asp.Net Core requests.
    /// </summary>
    [Route("services")]
    public class MiriController : Controller
    {
        private readonly IMiriServiceBus serviceBus;

        /// <summary>
        /// Creates a MiriController.
        /// </summary>
        public MiriController(IMiriServiceBus serviceBus)
        {
            // HINT: controller is not instantiated by DI, only the constructor arguments,
            // because controller is a concrete class and not registered in DI-container
            this.serviceBus = serviceBus;
        }

        /// <summary>
        /// Handles all GET-requests, e.g. "http://localhost:5000/services/BarRequest?stringvalue=HelloWorld".
        /// </summary>
        /// <param name="requestName">name of request type</param>
        /// <param name="request">content of request as query parameters</param>
        /// <returns>response as json result</returns>
        [HttpGet("{requestName}")]
        public async Task<IActionResult> Get([FromRoute] string requestName, [FromQuery] object request)
        {
            try
            {
                object response = await this.serviceBus.GetAsync(request);
                return Json(response);
            }
            catch (Exception ex)
            {
                return GetFailureObjectResult(ex);
            }
        }

        /// <summary>
        /// Handles all PUT-requests.
        /// </summary>
        /// <param name="requestName">name of request type</param>
        /// <param name="request">content of request as body parameters</param>
        /// <returns>response as json result</returns>
        [HttpPut("{requestName}")]
        public async Task<IActionResult> Put([FromRoute] string requestName, [FromBody] object request)
        {
            try
            {
                object response = await this.serviceBus.PutAsync(request);
                return Json(response);
            }
            catch (Exception ex)
            {
                return GetFailureObjectResult(ex);
            }
        }

        /// <summary>
        /// Handles all POST-requests.
        /// </summary>
        /// <param name="requestName">name of request type</param>
        /// <param name="request">content of request as body parameters</param>
        /// <returns>response as json result</returns>
        [HttpPost("{requestName}")]
        public async Task<IActionResult> Post([FromRoute] string requestName, [FromBody] object request)
        {
            try
            {
                object response = await this.serviceBus.PostAsync(request);
                return Json(response);
            }
            catch (Exception ex)
            {
                return GetFailureObjectResult(ex);
            }
        }

        /// <summary>
        /// Handles all DELETE-requests.
        /// </summary>
        /// <param name="requestName">name of request type</param>
        /// <param name="request">content of request as query parameters</param>
        /// <returns>response as json result</returns>
        [HttpDelete("{requestName}")]
        public async Task<IActionResult> Delete([FromRoute] string requestName, [FromQuery] object request)
        {
            try
            {
                object response = await this.serviceBus.DeleteAsync(request);
                return Json(response);
            }
            catch (Exception ex)
            {
                return GetFailureObjectResult(ex);
            }
        }

        private ObjectResult GetFailureObjectResult(Exception exception)
        {
            if (exception is RequestFailedException rex)
            {
                int statusCode = rex.StatusCode ?? StatusCodes.Status500InternalServerError;
                return StatusCode(statusCode, rex.ErrorResponse);
            }

            return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
        }
    }
}
