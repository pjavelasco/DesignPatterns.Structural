using System;
using static System.Console;

namespace Structural.Proxy.Protection
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            ICar car = new CarProxy(new Driver(12)); // 22
            car.Drive();
        }
    }

    public interface ICar
    {
        void Drive();
    }

    public class Car : ICar
    {
        public void Drive() => WriteLine("Car being driven");
    }

    public class Driver
    {
        public int Age { get; set; }

        public Driver(int age) => Age = age;
    }

    public class CarProxy : ICar
    {
        private readonly Car _car = new Car();
        private readonly Driver _driver;

        public CarProxy(Driver driver) => _driver = driver;

        public void Drive()
        {
            if (_driver.Age >= 16)
                _car.Drive();
            else
            {
                WriteLine("Driver too young");
            }
        }
    }
}
