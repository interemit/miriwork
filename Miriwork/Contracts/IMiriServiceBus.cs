using System;
using System.Threading.Tasks;

namespace Miriwork.Contracts
{
    /// <summary>
    /// Servicebus for forwarding a request to a bounded context.
    /// </summary>
    public interface IMiriServiceBus
    {
        /// <summary>
        /// Send a request as "GET".
        /// </summary>
        /// <param name="request">request for an application service</param>
        /// <returns>response of request</returns>
        Task<object> GetAsync(object request);

        /// <summary>
        /// Send a request as "PUT"
        /// </summary>
        /// <param name="request">request for an application service</param>
        /// <returns>response of request</returns>
        Task<object> PutAsync(object request);

        /// <summary>
        /// Send a request as "POST"
        /// </summary>
        /// <param name="request">request for an application service</param>
        /// <returns>response of request</returns>
        Task<object> PostAsync(object request);

        /// <summary>
        /// Send a request as "DELETE".
        /// </summary>
        /// <param name="request">request for an application service</param>
        /// <returns>response of request</returns>
        Task<object> DeleteAsync(object request);

        /// <summary>
        /// Invoked if an application service of a bounded context is created.
        /// </summary>
        event EventHandler<ApplicationServiceCreatedArgs> ApplicationServiceCreated;

        /// <summary>
        /// Invoked if the request was handled by the application service successfully.
        /// </summary>
        event EventHandler<RequestSuccessfulArgs> RequestSuccessful;

        /// <summary>
        /// Invoked if the request could not be handled by the application service.
        /// </summary>
        event EventHandler<RequestFailedArgs> RequestFailed;
    }

    /// <summary>
    /// Arguments for the ApplicationServiceCreated-event.
    /// </summary>
    public class ApplicationServiceCreatedArgs
    {
        /// <summary>
        /// Request which will be handled by the service.
        /// </summary>
        public object Request { get; }

        /// <summary>
        /// Application Service which handles the request.
        /// </summary>
        public object ApplicationService { get; }

        /// <summary>
        /// Current RequestContext.
        /// </summary>
        public RequestContext CurrentRequestContext { get; }

        /// <summary>
        /// Creates ApplicationServiceCreatedArgs.
        /// </summary>
        internal ApplicationServiceCreatedArgs(object request, object applicationService, RequestContext currentRequestContext)
        {
            Request = request;
            ApplicationService = applicationService;
            CurrentRequestContext = currentRequestContext;
        }
    }

    /// <summary>
    /// Arguments for the RequestSuccessful-event.
    /// </summary>
    public class RequestSuccessfulArgs
    {
        /// <summary>
        /// Request which was handled by the application service.
        /// </summary>
        public object Request { get; }

        /// <summary>
        /// Response of the request.
        /// </summary>
        public object Response { get; }

        /// <summary>
        /// Creates RequestSuccessfulArgs.
        /// </summary>
        internal RequestSuccessfulArgs(object request, object response)
        {
            Request = request;
            Response = response;
        }
    }

    /// <summary>
    /// Arguments for the RequestFailed-event.
    /// </summary>
    public class RequestFailedArgs
    {
        /// <summary>
        /// Request which could not be handled by the application service.
        /// </summary>
        public object Request { get; }

        /// <summary>
        /// Exception which occured during handling of request.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Response which should be send to client if request could not be handled (optional).
        /// </summary>
        public object ErrorResponse { get; set; }

        /// <summary>
        /// Statuscode whoch should be returned to client. If not set, StatusCodes.Status500InternalServerError will be used.
        /// </summary>
        public int? StatusCode { get; set; }

        /// <summary>
        /// Creates RequestFailedArgs without statuscode.
        /// </summary>
        /// <param name="request">Request which could not be handled</param>
        /// <param name="exception">Exception which occured</param>
        internal RequestFailedArgs(object request, Exception exception)
        {
            this.Request = request;
            this.Exception = exception;
        }

        /// <summary>
        /// Creates RequestFailedArgs with statuscode.
        /// </summary>
        /// <param name="request">Request which could not be handled</param>
        /// <param name="exception">Exception which occured</param>
        /// <param name="statusCode">Statuscode which should be returned to client</param>
        internal RequestFailedArgs(object request, Exception exception, int statusCode)
        {
            this.Request = request;
            this.Exception = exception;
            this.StatusCode = statusCode;
        }
    }
}