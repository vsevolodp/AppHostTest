namespace Contracts
{
    public interface ILogger
    {
        void Write(string format, params object[] args);
    }
}
