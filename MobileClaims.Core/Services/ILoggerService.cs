namespace MobileClaims.Core.Services
{
    public interface ILoggerService
    {
        void WriteLine(string line);
        string AH { get; }
        string AHX { get; }
    }
}
