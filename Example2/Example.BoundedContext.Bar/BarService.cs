using System.Threading.Tasks;
using Example.Webhosting;
using Example.Webhosting.Servicemodel;

namespace Example.BoundedContext.Bar
{
    public class BarService : IApplicationService
    {
        private readonly IApplicationServiceBus serviceBus;
        private readonly ISomeBarClass someBarClass;

        public BarService(IApplicationServiceBus serviceBus, ISomeBarClass someBarClass)
        {
            this.serviceBus = serviceBus;
            this.someBarClass = someBarClass;
        }

        public async Task<BarResponse> Get(BarRequest request)
        {
            // do some await...

            return new BarResponse
                {
                    StringValue = request.StringValue,
                    IntValue = request.IntValue
                };
        }

        public async Task<BarResponse> Put(BarRequest request)
        {
            // do some await...

            return new BarResponse
                {
                    StringValue = request.StringValue,
                    IntValue = request.IntValue
                };
        }

        public async Task<BarResponse> Post(BarRequest request)
        {
            // do some await...

            return new BarResponse
                {
                    StringValue = request.StringValue,
                    IntValue = request.IntValue
                };
        }

        public async Task<BarResponse> Delete(BarRequest request)
        {
            // do some await...

            return new BarResponse
                {
                    StringValue = request.StringValue,
                    IntValue = request.IntValue
                };
        }
    }

    
}