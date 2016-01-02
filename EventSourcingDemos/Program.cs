using System;
using EventSourcingDemos.Demos;
using EventSourcingDemos.Demos.Aggregates;
using EventSourcingDemos.Demos.Subscriptions;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace EventSourcingDemos
{
    // TODO: Spike out logging via an event store
    class Program
    {
        static void Main()
        {
            ConfigureLogging();

            // Demo demo = new CompetingConsumersSubscriptionDemo();
            // Demo demo = new CatchupSubscriptionDemo();
            // Demo demo = new LiveOnlySubscriptionDemo();
            Demo demo = new AggregateDemo();

            RunDemo(demo);
        }

        private static void RunDemo(Demo demo)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Running demo: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(demo.GetType().Name);
            Console.WriteLine();

            demo.Start().Wait();
        }

        #region Logging Setup

        private static void ConfigureLogging()
        {
            LoggingConfiguration config = new LoggingConfiguration();
            var consoleTarget = new ColoredConsoleTarget
            {
                Layout = @"${message}"
            };

            consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule
            {
                ForegroundColor = ConsoleOutputColor.Yellow,
                Condition = "level == LogLevel.Warn"
            });

            consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule
            {
                ForegroundColor = ConsoleOutputColor.Red,
                Condition = "level == LogLevel.Error"
            });

            consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule
            {
                ForegroundColor = ConsoleOutputColor.Green,
                Condition = "level == LogLevel.Info"
            });

            consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule
            {
                ForegroundColor = ConsoleOutputColor.White,
                Condition = "level < LogLevel.Info"
            });

            config.AddTarget("console", consoleTarget);

            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Trace, consoleTarget));

            LogManager.Configuration = config;
        }

        #endregion

    }
}
