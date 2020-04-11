namespace SRML
{
    public class ModVersion
    {
    }

    public class ModVersionRequirement
    {
        public ModVersion Version;
        public VersionCompareType Comparer;
    }

    public enum VersionCompareType
    {
        EqualTo = 1,
        GreaterThan = 2,
        LessThan = 4,
        Major = 8,
        Minor = 16,
        Revision=32
    }
}