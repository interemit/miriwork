using Example.Webhosting;
using Example.Webhosting.Servicemodel;

namespace Example.BoundedContext.Foo
{
    public class FooService : IApplicationService
    {
        private readonly ISomeFooClass someFooClass;

        public FooService(ISomeFooClass someFooClass)
        {
            this.someFooClass = someFooClass;
        }

        public FooResponse Get(FooRequest request)
        {
            return new FooResponse
                {
                    StringValue2 = request.StringValue2,
                    IntValue2 = request.IntValue2
                };
        }

        public FooResponse Put(FooRequest request)
        {
            return new FooResponse
                {
                    StringValue2 = request.StringValue2,
                    IntValue2 = request.IntValue2
                };
        }

        public FooResponse Post(FooRequest request)
        {
            return new FooResponse
                {
                    StringValue2 = request.StringValue2,
                    IntValue2 = request.IntValue2
                };
        }

        public FooResponse Delete(FooRequest request)
        {
            return new FooResponse
                {
                    StringValue2 = request.StringValue2,
                    IntValue2 = request.IntValue2
                };
        }
    }

    
}