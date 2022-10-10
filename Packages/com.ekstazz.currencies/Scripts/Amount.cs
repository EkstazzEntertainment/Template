namespace Ekstazz.Currencies
{
    using System;


    public readonly struct Amount : IEquatable<Amount>, IComparable<Amount>, IComparable
    {
        private static readonly string[] Postfixes = {"", "k", "M", "B", "T", "P", "E", "Z", "Y", "Aa", "Bb", "Cc", "Dd", "Ee", "Ff", "Gg"};
        
        private readonly double value;

        
        private Amount(double value)
        {
            this.value = value;
        }

        public static implicit operator Amount(int value)
        {
            return new Amount(value);
        }

        public static implicit operator double(Amount value)
        {
            return value.value;
        }

        public static implicit operator Amount(double value)
        {
            return new Amount(value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is Amount other && Equals(other);
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public static bool operator ==(Amount left, Amount right)
        {
            return left.Equals(right);
        }

        public static Amount operator +(Amount left, Amount right)
        {
            return new Amount(left.value + right.value);
        }
        
        public static Amount operator *(Amount left, double right)
        {
            return new Amount(left.value * right);
        }
        
        public static Amount operator /(Amount left, double right)
        {
            return new Amount(left.value / right);
        }

        public static Amount operator -(Amount left, Amount right)
        {
            return new Amount(left.value - right.value);
        }

        public static bool operator !=(Amount left, Amount right)
        {
            return !left.Equals(right);
        }

        public int CompareTo(Amount other)
        {
            return value.CompareTo(other.value);
        }

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return 1;
            }

            return obj is Amount other
                ? CompareTo(other)
                : throw new ArgumentException($"Object must be of type {nameof(Amount)}");
        }

        public static bool operator <(Amount left, Amount right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator >(Amount left, Amount right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator <=(Amount left, Amount right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >=(Amount left, Amount right)
        {
            return left.CompareTo(right) >= 0;
        }

        public bool Equals(Amount other)
        {
            return value.Equals(other.value);
        }

        public override string ToString()
        {
            var postfix = "";
            var order = 0;
            var buffer = value;

            if (buffer >= 1000)
            {
                while (Math.Round(buffer) >= 1000)
                {
                    buffer /= 1000;
                    order++;
                }

                if (order >= Postfixes.Length)
                {
                    return "Infinity";
                }

                postfix = Postfixes[order];
            }

            if (order == 0)
            {
                return ((int)buffer).ToString();
            }

            return $"{(Math.Round(buffer * 100) / 100):F2}{postfix}";
        }
    }

    public static class AmountExt 
    {
        public static int ToInt(this Amount amount)
        {
            return (int) amount;
        }
    }
}