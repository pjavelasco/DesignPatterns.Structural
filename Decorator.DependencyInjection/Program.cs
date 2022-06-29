using Autofac;
using System;

namespace Structural.Decorator.DependencyInjection
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            var b = new ContainerBuilder();
            b.RegisterType<ReportingService>().Named<IReportingService>("reporting");
            b.RegisterDecorator<IReportingService>(
                (context, service) => new ReportingServiceWithLogging(service),
              "reporting");

            // open generic decorators also supported
            // b.RegisterGenericDecorator()

            using (var c = b.Build())
            {
                var r = c.Resolve<IReportingService>();
                r.Report();
            }
        }
    }

    public interface IReportingService
    {
        void Report();
    }

    public class ReportingService : IReportingService
    {
        public void Report() => Console.WriteLine("Here is your report");
    }

    public class ReportingServiceWithLogging : IReportingService
    {
        private readonly IReportingService _decorated;

        public ReportingServiceWithLogging(IReportingService decorated)
        {
            if (decorated == null)
            {
                throw new ArgumentNullException(paramName: nameof(decorated));
            }
            _decorated = decorated;
        }

        public void Report()
        {
            Console.WriteLine("Commencing log...");
            _decorated.Report();
            Console.WriteLine("Ending log...");
        }
    }
}
