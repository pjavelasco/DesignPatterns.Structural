using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using ImpromptuInterface;
using static System.Console;

namespace Structural.Proxy.Dynamic
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            //var ba = new BankAccount();
            var ba = Log<BankAccount>.As<IBankAccount>();

            ba.Deposit(100);
            ba.Withdraw(50);

            WriteLine(ba);
        }
    }

    public interface IBankAccount
    {
        void Deposit(int amount);
        bool Withdraw(int amount);
        string ToString();
    }

    public class BankAccount : IBankAccount
    {
        private int _balance;
        private readonly int _overdraftLimit = -500;

        public void Deposit(int amount)
        {
            _balance += amount;
            WriteLine($"Deposited ${amount}, balance is now {_balance}");
        }

        public bool Withdraw(int amount)
        {
            if (_balance - amount >= _overdraftLimit)
            {
                _balance -= amount;
                WriteLine($"Withdrew ${amount}, balance is now {_balance}");
                return true;
            }
            return false;
        }

        public override string ToString() => $"{nameof(_balance)}: {_balance}";
    }

    public class Log<T> : DynamicObject where T : class, new()
    {
        private readonly T _subject;
        private readonly Dictionary<string, int> _methodCallCount = new();

        protected Log(T subject) => _subject = subject ?? throw new ArgumentNullException(paramName: nameof(subject));

        // factory method
        public static I As<I>(T subject) where I : class
        {
            if (!typeof(I).IsInterface)
            {
                throw new ArgumentException("I must be an interface type");
            }

            // duck typing here!
            return new Log<T>(subject).ActLike<I>();
        }

        public static I As<I>() where I : class
        {
            if (!typeof(I).IsInterface)
            {
                throw new ArgumentException("I must be an interface type");
            }

            // duck typing here!
            return new Log<T>(new T()).ActLike<I>();
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            try
            {
                // logging
                WriteLine($"Invoking {_subject.GetType().Name}.{binder.Name} with arguments [{string.Join(",", args)}]");

                // more logging
                if (_methodCallCount.ContainsKey(binder.Name))
                {
                    _methodCallCount[binder.Name]++;
                }
                else 
                { 
                    _methodCallCount.Add(binder.Name, 1); 
                }

                result = _subject.GetType().GetMethod(binder.Name).Invoke(_subject, args);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        public string Info
        {
            get
            {
                var sb = new StringBuilder();
                foreach (var kv in _methodCallCount)
                {
                    sb.AppendLine($"{kv.Key} called {kv.Value} time(s)");
                }
                return sb.ToString();
            }
        }

        // will not be proxied automatically
        public override string ToString() => $"{Info}{_subject}";
    }
}
