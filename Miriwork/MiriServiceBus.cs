using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Miriwork.Contracts;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace Miriwork
{
    internal class MiriServiceBus : IMiriServiceBus
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IRequestContextAccessor requestContextAccessor;
        private readonly Dictionary<Type, Type> requestType2ApplicationServiceType;

        public event EventHandler<ApplicationServiceCreatedArgs> ApplicationServiceCreated;
        public event EventHandler<RequestSuccessfulArgs> RequestSuccessful;
        public event EventHandler<RequestFailedArgs> RequestFailed;

        public MiriServiceBus(IHttpContextAccessor httpContextAccessor, IRequestContextAccessor requestContextAccessor,
            Dictionary<Type, Type> requestType2ApplicationServiceType)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.requestContextAccessor = requestContextAccessor;
            this.requestType2ApplicationServiceType = requestType2ApplicationServiceType;
        }

        public async Task<object> GetAsync(object request)
        {
            return await HandleRequest(request, HttpMethod.Get);
        }

        public async Task<object> PutAsync(object request)
        {
            return await HandleRequest(request, HttpMethod.Put);
        }

        public async Task<object> PostAsync(object request)
        {
            return await HandleRequest(request, HttpMethod.Post);
        }

        public async Task<object> DeleteAsync(object request)
        {
            return await HandleRequest(request, HttpMethod.Delete);
        }

        private async Task<object> HandleRequest(object request, HttpMethod httpMethod)
        {
            Type requestType = request.GetType();
            Type applicationServiceType = this.requestType2ApplicationServiceType[requestType];

            // create application service (must be registered as dependency)
            object applicationService = httpContextAccessor.HttpContext.RequestServices.GetRequiredService(applicationServiceType);

            this.ApplicationServiceCreated?.Invoke(this, new ApplicationServiceCreatedArgs(
                request: request,
                applicationService: applicationService,
                currentRequestContext: this.requestContextAccessor.RequestContext
            ));

            return await GetResponse(request, applicationService, applicationServiceType, httpMethod);
        }

        private async Task<object> GetResponse(object request, object applicationService, Type applicationServiceType, 
            HttpMethod httpMethod)
        {
            try
            {
                MethodInfo methodInfo = applicationServiceType.GetMethod(httpMethod.ToString());
                var result = methodInfo.Invoke(applicationService, new[] { request });

                object response;
                if (IsAsyncMethod(methodInfo))
                    response = await (dynamic)result;
                else
                    response = result;

                this.RequestSuccessful?.Invoke(this, new RequestSuccessfulArgs(request: request, response: response));

                return response;
            }
            catch (TargetInvocationException tex)
            {
                Exception ex = UnwrapTargetInvocationException(tex);
                Exception requestFailedException = CreateRequestFailedException(request, ex);
                throw requestFailedException;
            }
            catch (Exception ex)
            {
                Exception requestFailedException = CreateRequestFailedException(request, ex);
                throw requestFailedException;
            }
        }
        
        private bool IsAsyncMethod(MethodInfo methodInfo)
        {
            var attrib = (AsyncStateMachineAttribute)methodInfo.GetCustomAttribute(typeof(AsyncStateMachineAttribute));
            return (attrib != null);
        }

        private Exception UnwrapTargetInvocationException(TargetInvocationException exception)
        {
            try
            {
                ExceptionDispatchInfo.Capture(exception.InnerException).Throw();
                return exception;   // returned value does not matter
            }
            catch (Exception combinedEx)
            {
                return combinedEx;
            }
        }

        private RequestFailedException CreateRequestFailedException(object request, Exception exception)
        {
            RequestFailedArgs requestFailedArgs = new RequestFailedArgs(request, exception);
            this.RequestFailed?.Invoke(this, requestFailedArgs);

            object errorResponse = requestFailedArgs.ErrorResponse ?? new { Message = exception.Message };
            throw new RequestFailedException(errorResponse, requestFailedArgs.StatusCode);
        }
    }
} 