using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flyweight
{
    public class User
    {
        private readonly string _fullName;

        public User(string fullName) => _fullName = fullName;
    }

    public class User2
    {
        static List<string> _strings = new List<string>();
        private readonly int[] _names;

        public User2(string fullName)
        {
            static int getOrAdd(string s)
            {
                int idx = _strings.IndexOf(s);
                if (idx != -1) return idx;
                else
                {
                    _strings.Add(s);
                    return _strings.Count - 1;
                }
            }

            _names = fullName.Split(' ').Select(getOrAdd).ToArray();
        }

        public string FullName => string.Join(" ", _names);
    }
}
