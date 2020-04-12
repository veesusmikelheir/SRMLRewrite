namespace SRML.ModLoading.API
{
    public interface IModDependency
    {
        string ID { get; }
        VersionRequirement VersionRequirement { get; }
    }

}