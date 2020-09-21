namespace DurableInjectOptions
{
    public interface IMyConfig
    {
        int Timeout { get; set; }
        string Endpoint { get; set; }

    }
}