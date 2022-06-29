using System;

namespace Structural.Decorator.DynamicComposition
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            var square = new Square(1.23f);
            Console.WriteLine(square.AsString());

            var redSquare = new ColoredShape(square, "red");
            Console.WriteLine(redSquare.AsString());

            var redHalfTransparentSquare = new TransparentShape(redSquare, 0.5f);
            Console.WriteLine(redHalfTransparentSquare.AsString());

            // static
            ColoredShape<Circle> blueCircle = new ColoredShape<Circle>("blue");
            Console.WriteLine(blueCircle.AsString());

            TransparentShape<ColoredShape<Square>> blackHalfSquare = new TransparentShape<ColoredShape<Square>>(0.4f);
            Console.WriteLine(blackHalfSquare.AsString());
        }
    }

    public abstract class Shape
    {
        public virtual string AsString() => string.Empty;
    }

    public class Circle : Shape
    {
        private float _radius;

        public Circle() : this(0)
        {
        }

        public Circle(float radius) => _radius = radius;

        public void Resize(float factor) => _radius *= factor;

        public override string AsString() => $"A circle of radius {_radius}";
    }

    public class Square : Shape
    {
        private float _side;

        public Square() : this(0)
        {
        }

        public Square(float side) => _side = side;

        public override string AsString() => $"A square with side {_side}";
    }

    // dynamic
    public class ColoredShape : Shape
    {
        private readonly Shape _shape;
        private readonly string _color;

        public ColoredShape(Shape shape, string color)
        {
            _shape = shape ?? throw new ArgumentNullException(paramName: nameof(shape));
            _color = color ?? throw new ArgumentNullException(paramName: nameof(color));
        }

        public override string AsString() => $"{_shape.AsString()} has the color {_color}";
    }

    public class TransparentShape : Shape
    {
        private readonly Shape _shape;
        private readonly float _transparency;

        public TransparentShape(Shape shape, float transparency)
        {
            this._shape = shape ?? throw new ArgumentNullException(paramName: nameof(shape));
            this._transparency = transparency;
        }

        public override string AsString() => $"{_shape.AsString()} has {_transparency * 100.0f} transparency";
    }

    // CRTP cannot be done
    //public class ColoredShape2<T> : T where T : Shape { }

    public class ColoredShape<T> : Shape where T : Shape, new()
    {
        private readonly string _color;
        private readonly T _shape = new T();

        public ColoredShape() : this("black")
        {
        }

        public ColoredShape(string color) // no constructor forwarding
        {
            _color = color ?? throw new ArgumentNullException(paramName: nameof(color));
        }

        public override string AsString() => $"{_shape.AsString()} has the color {_color}";
    }

    public class TransparentShape<T> : Shape where T : Shape, new()
    {
        private readonly float _transparency;
        private readonly T _shape = new T();

        public TransparentShape(float transparency) => _transparency = transparency;

        public override string AsString() => $"{_shape.AsString()} has transparency {_transparency * 100.0f}";
    }
}
