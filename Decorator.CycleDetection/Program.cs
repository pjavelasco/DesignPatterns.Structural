﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Structural.Decorator.CycleDetection
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            var circle = new Circle(2);
            var colored1 = new ColoredShape(circle, "red");
            var colored2 = new ColoredShape(colored1, "blue");

            Console.WriteLine(circle.AsString());
            Console.WriteLine(colored1.AsString());
            Console.WriteLine(colored2.AsString());
        }
    }

    public abstract class Shape
    {
        public virtual string AsString() => string.Empty;
    }

    public sealed class Circle : Shape
    {
        private float _radius;

        public Circle() : this(0)
        {
        }

        public Circle(float radius) => _radius = radius;

        public void Resize(float factor) => _radius *= factor;

        public override string AsString() => $"A circle of radius {_radius}";
    }

    public sealed class Square : Shape
    {
        private readonly float _side;

        public Square() : this(0)
        {
        }

        public Square(float side) => _side = side;

        public override string AsString() => $"A square with side {_side}";
    }

    public abstract class ShapeDecoratorCyclePolicy
    {
        public abstract bool TypeAdditionAllowed(Type type, IList<Type> allTypes);
        public abstract bool ApplicationAllowed(Type type, IList<Type> allTypes);
    }

    public class ThrowOnCyclePolicy : ShapeDecoratorCyclePolicy
    {
        private bool Handler(Type type, IList<Type> allTypes)
        {
            if (allTypes.Contains(type))
            {
                throw new InvalidOperationException($"Cycle detected! Type is already a {type.FullName}!");
            }
            return true;
        }

        public override bool TypeAdditionAllowed(Type type, IList<Type> allTypes) => Handler(type, allTypes);

        public override bool ApplicationAllowed(Type type, IList<Type> allTypes) => Handler(type, allTypes);
    }

    public class AbsorbCyclePolicy : ShapeDecoratorCyclePolicy
    {
        public override bool TypeAdditionAllowed(Type type, IList<Type> allTypes) => true;

        public override bool ApplicationAllowed(Type type, IList<Type> allTypes) => !allTypes.Contains(type);
    }

    public class CyclesAllowedPolicy : ShapeDecoratorCyclePolicy
    {
        public override bool TypeAdditionAllowed(Type type, IList<Type> allTypes) => true;

        public override bool ApplicationAllowed(Type type, IList<Type> allTypes) => true;
    }

    public abstract class ShapeDecorator : Shape
    {
        protected internal readonly List<Type> types = new();
        protected internal Shape shape;

        public ShapeDecorator(Shape shape)
        {
            this.shape = shape;
            if (shape is ShapeDecorator sd)
            {
                types.AddRange(sd.types);
            }
        }
    }

    public abstract class ShapeDecorator<TSelf, TCyclePolicy> : ShapeDecorator
      where TCyclePolicy : ShapeDecoratorCyclePolicy, new()
    {
        protected readonly TCyclePolicy policy = new();

        public ShapeDecorator(Shape shape) : base(shape)
        {
            if (policy.TypeAdditionAllowed(typeof(TSelf), types))
            {
                types.Add(typeof(TSelf));
            }
        }
    }

    // can determine one policy for all classes
    public class ShapeDecoratorWithPolicy<T>
      : ShapeDecorator<T, ThrowOnCyclePolicy>
    {
        public ShapeDecoratorWithPolicy(Shape shape) : base(shape)
        {
        }
    }

    // dynamic
    public class ColoredShape
      : ShapeDecorator<ColoredShape, AbsorbCyclePolicy>
    {

        private readonly string color;

        public ColoredShape(Shape shape, string color) : base(shape)
        {
            this.color = color;
        }

        public override string AsString()
        {
            var sb = new StringBuilder($"{shape.AsString()}");

            if (policy.ApplicationAllowed(types[0], types.Skip(1).ToList()))
                sb.Append($" has the color {color}");

            return sb.ToString();
        }
    }

}
