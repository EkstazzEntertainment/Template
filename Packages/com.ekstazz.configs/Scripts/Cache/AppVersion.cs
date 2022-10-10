namespace Ekstazz.Configs.Cache
{
    using System;

    
    internal readonly struct AppVersion : IComparable<AppVersion>
    {
        public readonly int major;
        public readonly int minor;
        public readonly int build;
        
        
        public AppVersion(int major, int minor, int build)
        {
            this.major = major;
            this.minor = minor;
            this.build = build;
        }
        
        public override bool Equals(object obj)
        {
            return obj is AppVersion other && Equals(other);
        }
        
        private bool Equals(AppVersion other)
        {
            return major == other.major && minor == other.minor && build == other.build;
        }
        
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = major;
                hashCode = (hashCode * 397) ^ minor;
                hashCode = (hashCode * 397) ^ build;
                return hashCode;
            }
        }
        
        public static bool operator ==(AppVersion left, AppVersion right)
        {
            return left.Equals(right);
        }
        
        public static bool operator !=(AppVersion left, AppVersion right)
        {
            return !left.Equals(right);
        }

        public static bool operator <(AppVersion left, AppVersion right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator >(AppVersion left, AppVersion right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator <=(AppVersion left, AppVersion right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >=(AppVersion left, AppVersion right)
        {
            return left.CompareTo(right) >= 0;
        }

        public int CompareTo(AppVersion other)
        {
            var majorComparison = major.CompareTo(other.major);
            if (majorComparison != 0)
            {
                return majorComparison;
            }
            var minorComparison = minor.CompareTo(other.minor);
            if (minorComparison != 0)
            {
                return minorComparison;
            }
            return build.CompareTo(other.build);
        }
        
        public override string ToString()
        {
            return $"{major}.{minor}.{build}";
        }
    }
}