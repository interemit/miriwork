using Example.Simple.Servicemodel;

namespace Example.Simple
{
    public class SimpleApplicationService
    {
        public SimpleResponse Get(SimpleRequest request)
        {
            return new SimpleResponse
            {
                SimpleString = request.SimpleString
            };
        }

        public SimpleResponse Put(SimpleRequest request)
        {
            return new SimpleResponse
            {
                SimpleString = request.SimpleString
            };
        }

        public SimpleResponse Post(SimpleRequest request)
        {
            return new SimpleResponse
            {
                SimpleString = request.SimpleString
            };
        }

        public SimpleResponse Delete(SimpleRequest request)
        {
            return new SimpleResponse
            {
                SimpleString = request.SimpleString
            };
        }
    }
}
