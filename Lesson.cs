using System;
using System.IO;

namespace Lesson
{
    class Program
    {
        static void Main(string[] args)
        {
            string logMessageToFile = LogConstants.LogMessageToFile;
            string logMessageToConsole = LogConstants.LogMessageToConsole;
            string logMessageToFileOnFriday = LogConstants.LogMessageToFileOnFriday;
            string logMessageToConsoleOnFriday = LogConstants.LogMessageToConsoleOnFriday;
            string logMessageToConsoleAndFileOnFriday = LogConstants.LogMessageToConsoleAndFileOnFriday;

            ILogger fileLogger = new FileLogWriter();
            ILogger consoleLogger = new ConsoleLogWriter();

            fileLogger.Log($"{DateTime.Now}: {logMessageToFile}");

            consoleLogger.Log($"{DateTime.Now}: {logMessageToConsole}");

            ILogger fileLoggerOnFriday = new FridayLogger(fileLogger);
            fileLoggerOnFriday.Log($"{DateTime.Now}: {logMessageToFileOnFriday}");

            ILogger consoleLoggerOnFriday = new FridayLogger(consoleLogger);
            consoleLoggerOnFriday.Log($"{DateTime.Now}: {logMessageToConsoleOnFriday}");

            ILogger consoleAndFileLoggerOnFriday = new FridayLogger(new CombinedLogger(
                fileLogger,
                consoleLogger
            ));
            consoleAndFileLoggerOnFriday.Log($"{DateTime.Now}: {logMessageToConsoleAndFileOnFriday}");
        }
    }

    public interface ILogger
    {
        void Log(string message);
    }

    public class FridayLogger : ILogger
    {
        private readonly ILogger _logger;

        public FridayLogger(ILogger logger)
        {
            _logger = logger;
        }

        public void Log(string message)
        {
            if (DateTime.Now.DayOfWeek == LogConstants.TargetDay)
            {
                _logger.Log(message);
            }
        }
    }

    public class CombinedLogger : ILogger
    {
        private readonly ILogger[] _loggers;

        public CombinedLogger(params ILogger[] loggers)
        {
            _loggers = loggers;
        }

        public void Log(string message)
        {
            foreach (var logger in _loggers)
            {
                logger.Log(message);
            }
        }
    }

    public class FileLogWriter : ILogger
    {
        public void Log(string message)
        {
            File.AppendAllText(LogConstants.LogFilePath, message + Environment.NewLine);
        }
    }

    public class ConsoleLogWriter : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }

    public static class LogConstants
    {
        public const string LogFilePath = "log.txt";
        public const DayOfWeek TargetDay = DayOfWeek.Friday;

        public const string LogMessageToFile = "Это сообщение для файла";
        public const string LogMessageToConsole = "Это сообщение для консоли";
        public const string LogMessageToFileOnFriday = "Это сообщение для файла по пятницам";
        public const string LogMessageToConsoleOnFriday = "Это сообщение для консоли по пятницам";
        public const string LogMessageToConsoleAndFileOnFriday = "Это сообщение для консоли и файла по пятницам";
    }
}