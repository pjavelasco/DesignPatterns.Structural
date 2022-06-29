using System;
using System.Collections.Generic;

namespace Structural.Proxy.SoAComposite
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            //var creatures = new Creature[100];
            //foreach (var c in creatures)
            //{
            //    c.X++; // not memory-efficient
            //}

            var creatures2 = new Creatures(100);
            foreach (var c in creatures2)
            {
                c.X++;
            }
        }
    }

    class Creature
    {
        public byte Age { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }

    class Creatures
    {
        private readonly int _size;
        private readonly byte[] _age;
        private readonly int[] _x;
        private readonly int[] _y;

        public Creatures(int size)
        {
            _size = size;
            _age = new byte[size];
            _x = new int[size];
            _y = new int[size];
        }

        public struct CreatureProxy
        {
            private readonly Creatures _creatures;
            private readonly int _index;

            public CreatureProxy(Creatures creatures, int index)
            {
                _creatures = creatures;
                _index = index;
            }

            public ref byte Age => ref _creatures._age[_index];
            public ref int X => ref _creatures._x[_index];
            public ref int Y => ref _creatures._y[_index];
        }

        public IEnumerator<CreatureProxy> GetEnumerator()
        {
            for (int pos = 0; pos < _size; ++pos)
            {
                yield return new CreatureProxy(this, pos);
            }
        }
    }
}
