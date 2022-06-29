using System;
using System.Collections.Generic;
using static System.Console;

namespace Structural.Proxy.Property
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            var c = new Creature();
            c.Agility = 10; // c.set_Agility(10) xxxxxxxxxxxxx
                            // c.Agility = new Property<int>(10)
            c.Agility = 10;
        }
    }

    public class Property<T> : IEquatable<Property<T>> where T : new()
    {
        private T _value;

        public T Value
        {
            get => _value;
            set
            {
                if (Equals(_value, value))
                {
                    return;
                }
                WriteLine($"Assigning value to {value}");
                _value = value;
            }
        }

        public Property() : this(default)
        {
        }

        public Property(T value) => _value = value;

        public static implicit operator T(Property<T> property)
        {
            return property._value; // int n = p_int;
        }

        public static implicit operator Property<T>(T value)
        {
            return new Property<T>(value); // Property<int> p = 123;
        }

        public bool Equals(Property<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return EqualityComparer<T>.Default.Equals(_value, other._value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Property<T>)obj);
        }

        public override int GetHashCode() => _value.GetHashCode();

        public static bool operator ==(Property<T> left, Property<T> right) => Equals(left, right);

        public static bool operator !=(Property<T> left, Property<T> right) => !Equals(left, right);
    }

    public class Creature
    {
        private readonly Property<int> _agility = new();

        public int Agility
        {
            get => _agility.Value;
            set => _agility.Value = value;
        }
    }
}
