namespace SRML.ModLoading.API
{
    public interface IModLoadOrder
    {
        string[] LoadBefore { get; }
        string[] LoadAfter { get; }
    }
}