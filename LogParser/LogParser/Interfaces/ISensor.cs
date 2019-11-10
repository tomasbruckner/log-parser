
namespace LogParser.Interfaces
{
    public interface ISensor
    {
        string CalculateQuality();
        string GetType();
        string GetName();
        void HandleValue(string value);
    }
}
