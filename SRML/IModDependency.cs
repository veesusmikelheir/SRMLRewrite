namespace SRML
{
    public interface IModDependency
    {
        string ID { get; }
        ModVersionRequirement VersionRequirement { get; }
    }

}