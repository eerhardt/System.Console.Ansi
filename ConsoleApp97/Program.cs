using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;

namespace ConsoleApp97
{
    class Program
    {
        static void Main(string[] args)
        {
            var supportedWriter = new LoggingTextWriter(ConsoleFormatInfo.AnsiSupportedInfo);
            var notSupportedWriter = new LoggingTextWriter(ConsoleFormatInfo.AnsiNotSupportedInfo);

            var consoleLogFormatter = new PrettyConsoleLogFormatter();
            consoleLogFormatter.Write<object>(LogLevel.Information, "CA100", default, new object(), null, (o, e) => "state string result", null, supportedWriter);
            Console.WriteLine();
            
            consoleLogFormatter.Write<object>(LogLevel.Information, "CA100", default, new object(), null, (o, e) => "state string result", null, notSupportedWriter);
            Console.WriteLine();

            // test writing to the console directly
            Console.WriteLine($"{Ansi.Color.Foreground.Green}Console test{Ansi.Color.Foreground.Default}");
        }
    }

    public partial interface IConsoleLogFormatter
    {
        string Name { get; }

        void Write<TState>(LogLevel logLevel, string category, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter, IExternalScopeProvider scopeProvider, TextWriter textWriter);
    }

    public class PrettyConsoleLogFormatter : IConsoleLogFormatter
    {
        public string Name => "Pretty";

        public void Write<TState>(LogLevel logLevel, string category, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter, IExternalScopeProvider scopeProvider, TextWriter writer)
        {
            writer.Write(Ansi.Color.Foreground.Red);
            writer.Write(logLevel);

            writer.Write(Ansi.Color.Foreground.White);
            writer.Write(category);

            writer.Write(Ansi.Color.Foreground.Blue);
            writer.Write(formatter.Invoke(state, exception));

            writer.Write(Ansi.Color.Foreground.Cyan);
            writer.Write(4.5f);

            writer.Write(Ansi.Color.Foreground.Default);

            writer.WriteFormat($"{Ansi.Color.Foreground.LightMagenta}John test{Ansi.Color.Foreground.Default}");
        }
    }

    public static class TextWriterExtensions
    {
        public static void WriteFormat(this TextWriter writer, FormattableString s)
        {
            writer.Write(s.ToString(writer.FormatProvider));
            //switch (s.ArgumentCount)
            //{
            //    case 0: writer.Write(s.Format); break;
            //    case 1: writer.Write(s.Format, Format(s.GetArgument(0))); break;
            //    case 2: writer.Write(s.Format, Format(s.GetArgument(0)), Format(s.GetArgument(1)));break;
            //    case 3: writer.Write(s.Format, Format(s.GetArgument(0)), Format(s.GetArgument(1)), Format(s.GetArgument(2)));break;
            //    default:
            //        object?[] args = s.GetArguments();
            //        object[] formattedArgs = new object[args.Length];
            //        for (int i = 0; i < args.Length; i++)
            //        {
            //            formattedArgs[i] = Format(args[i]);
            //        }
            //        writer.Write(s.Format, formattedArgs);
            //        break;
            //}


        }
    }

    class LoggingTextWriter : TextWriter
    {
        //private int _index;
        /// <summary>
        /// This should come from ArrayPool
        /// </summary>
        //private char[] _buffer = new char[512];

        public LoggingTextWriter(IFormatProvider formatProvider)
            : base(formatProvider)
        {

        }

        public override Encoding Encoding => Console.Out.Encoding;

        public override void Write(char value)
        {
            Console.Out.Write(value);

           // _buffer[_index] = value;
            //_index++;
        }
    }
}
