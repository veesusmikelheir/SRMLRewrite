namespace SRML
{
    public interface IModDependency
    {
        string ID { get; }
        VersionRequirement VersionRequirement { get; }
    }

}