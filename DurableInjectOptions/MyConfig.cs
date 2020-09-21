    namespace DurableInjectOptions
{
    public class MyConfig : IMyConfig
    {
        public int Timeout { get; set; }
        public string Endpoint { get; set; }
    }
}