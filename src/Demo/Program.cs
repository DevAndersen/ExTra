using ExTra;
using System;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            ODataTranslator exTra = new ODataTranslator();

            string translation = exTra.Translate((OuterClass outer) =>
                outer.OuterString == "Hello, world!"
                && outer.Middle.MiddleInteger < 5
                || outer.Middle.Inner.InnerDouble >= 10.2D
                || outer.OuterString.StartsWith("Once upon a time")
                || outer.OuterString.EndsWith("The End.")
                || "A constant value".StartsWith("A const")
            );

            Console.WriteLine($"$filter={translation}");
            Console.ReadLine();
        }
    }

    #region Demo classes

    public class OuterClass
    {
        public string OuterString { get; set; }

        public MiddleClass Middle { get; set; }
    }

    public class MiddleClass
    {
        public int MiddleInteger { get; set; }

        public InnerClass Inner { get; set; }
    }

    public class InnerClass
    {
        public double InnerDouble { get; set; }
    }

    #endregion
}
