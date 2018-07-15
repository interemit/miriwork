using System;
using System.Threading.Tasks;
using Example.Webhosting.Servicemodel;
using Microsoft.AspNetCore.Http;
using Miriwork.Contracts;

namespace Example.Webhosting
{
    public class ApplicationServiceBus : IApplicationServiceBus
    {
        private readonly IMiriServiceBus miriServiceBus;

        public ApplicationServiceBus(IMiriServiceBus miriServiceBus)
        {
            this.miriServiceBus = miriServiceBus;
            this.miriServiceBus.ApplicationServiceCreated += (sender, args) => 
            {
                Console.WriteLine($"Service called in {args.CurrentRequestContext.BoundedContextId}");
            };
            this.miriServiceBus.RequestSuccessful += (sender, args) => 
            {
                Console.WriteLine("Request successful");
            };
            this.miriServiceBus.RequestFailed += (sender, args) => 
            {
                Console.WriteLine($"Request failed with: {args.Exception.Message}");

                args.ErrorResponse = new Response
                    {
                        ErrorMessage = args.Exception.Message
                    };
                args.StatusCode = StatusCodes.Status500InternalServerError;
            };
        }

        public async Task<TResponse> GetAsync<TResponse>(object request)
        {
            return (TResponse)(await this.miriServiceBus.GetAsync(request));
        }

        public async Task<TResponse> PutAsync<TResponse>(object request)
        {
            return (TResponse)(await this.miriServiceBus.PutAsync(request));
        }

        public async Task<TResponse> PostAsync<TResponse>(object request)
        {
            return (TResponse)(await this.miriServiceBus.PostAsync(request));
        }

        public async Task<TResponse> DeleteAsync<TResponse>(object request)
        {
            return (TResponse)(await this.miriServiceBus.DeleteAsync(request));
        }
    }
}