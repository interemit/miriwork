namespace Example.Webhosting.Servicemodel
{
    public class FooRequest : IRequest
    {
        public string StringValue2 { get; set; }

        public int IntValue2 { get; set; }
    }

    public class FooResponse : Response
    {
        public string StringValue2 { get; set; }

        public int IntValue2 { get; set; }
    }
}