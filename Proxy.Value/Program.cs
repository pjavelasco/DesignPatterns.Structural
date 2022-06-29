using System;

namespace Structural.Proxy.Value
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(10f * 5.Percent());
            Console.WriteLine(2.Percent() + 3.Percent());
        }
    }

    public struct Percentage
    {
        private readonly float _value;

        internal Percentage(float value) => this._value = value;

        //    public static implicit operator Percentage(float value)
        //    {
        //      return new Percentage(value);
        //    }

        public static float operator *(float f, Percentage p) => f * p._value;

        public static Percentage operator +(Percentage a, Percentage b) => new Percentage(a._value + b._value);

        public static implicit operator Percentage(int value) => value.Percent();

        public bool Equals(Percentage other) => _value.Equals(other._value);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Percentage other && Equals(other);
        }

        public override int GetHashCode() => _value.GetHashCode();

        public override string ToString() => $"{_value * 100}%";
    }

    public static class PercentageExtensions
    {
        public static Percentage Percent(this int value) => new(value / 100.0f);

        public static Percentage Percent(this float value) => new(value / 100.0f);
    }
}
