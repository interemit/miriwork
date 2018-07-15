namespace Example.Webhosting.Servicemodel
{
    public class BarRequest : IRequest
    {
        public string StringValue { get; set; }

        public int IntValue { get; set; }
    }

    public class BarResponse : Response
    {
        public string StringValue { get; set; }

        public int IntValue { get; set; }
    }
}