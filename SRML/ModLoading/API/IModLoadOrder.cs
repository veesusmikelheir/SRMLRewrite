namespace SRML.ModLoading.API
{
    public interface IModLoadOrder
    {
        string[] LoadsBefore { get; }
        string[] LoadsAfter { get; }
    }
}