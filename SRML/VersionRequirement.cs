using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRML
{
    public class VersionRequirement
    {
        public Version ComparedVersion;
        public CompareType CompareType;

        public VersionRequirement(Version comparedVersion, CompareType compareType)
        {
            ComparedVersion = comparedVersion;
            CompareType = compareType;
        }

        public override string ToString()
        {
            return CompareType.GetString() + ComparedVersion.ToString();
        }

        public static VersionRequirement Parse(string requirement)
        {

            if (requirement == null) throw new ArgumentNullException("requirement");
            if (requirement.Length == 0) throw new ArgumentException("requirement");
            var control = String.Concat(requirement.TakeWhile(x => !char.IsLetterOrDigit(x)));

            var compareType = control.Length == 0 ? CompareType.EqualTo : CompareTypeExtensions.Parse(control);
            var version = Version.Parse(requirement.Remove(0, control.Length));
            return new VersionRequirement(version, compareType);
        }

        public override bool Equals(object obj)
        {
            return obj is VersionRequirement requirement &&
                   requirement.ComparedVersion.CompareTo(ComparedVersion)==0 &&
                   CompareType == requirement.CompareType;
        }

        public override int GetHashCode()
        {
            var hashCode = 539311416;
            hashCode = hashCode * -1521134295 + EqualityComparer<Version>.Default.GetHashCode(ComparedVersion);
            hashCode = hashCode * -1521134295 + CompareType.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(VersionRequirement left, VersionRequirement right)
        {
            return EqualityComparer<VersionRequirement>.Default.Equals(left, right);
        }

        public static bool operator !=(VersionRequirement left, VersionRequirement right)
        {
            return !(left == right);
        }
    }
    public enum CompareType
    {
        NONE=0,
        EqualTo = 1,
        GreaterThan = 2,
        LessThan = 4
    }

    public static class CompareTypeExtensions
    {
        public static CompareType Parse(string type)
        {
            if(type == null) throw new ArgumentNullException("type");
            type = type.Trim();
            if (type.Length == 0 || type.Length > 2) throw new ArgumentException("type");
            var outType = type.Contains("=") ? CompareType.EqualTo : CompareType.NONE;
            if (type.Contains(">")) outType |= CompareType.GreaterThan;
            else if (type.Contains("<")) outType |= CompareType.LessThan;
            return outType;
        }

        public static string GetString(this CompareType type)
        {
            if (type == CompareType.NONE) throw new ArgumentOutOfRangeException("type");
            if ((type & CompareType.GreaterThan) > 0 && (type & CompareType.LessThan) > 0) throw new ArgumentOutOfRangeException("type");
            var stringOut = "";
            if ((type & CompareType.GreaterThan) > 0) stringOut += ">";
            else if ((type & CompareType.LessThan) > 0) stringOut += "<";
            else stringOut += "=";
            if ((type & CompareType.EqualTo) > 0) stringOut += "=";
            return stringOut;

        }
    }
}
