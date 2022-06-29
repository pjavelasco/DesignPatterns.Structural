using System;

namespace Structural.Decorator.MultipleInheritance
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

        class Bird
        {
            public void Fly() { }
        }

        class Lizard
        {
            public void Crawl() { }
        }

        class Dragon
        {
            private readonly Bird _bird;
            private readonly Lizard _lizard;

            public Dragon(Bird bird, Lizard lizard)
            {
                _bird = bird ?? throw new ArgumentNullException(nameof(bird));
                _lizard = lizard ?? throw new ArgumentNullException(nameof(lizard));
            }

            public void Crawl() 
            {
                _lizard.Crawl();
            }

            public void Fly() 
            {
                _bird.Fly();
            }
        }
    }
}
