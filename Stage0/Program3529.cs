using System;
namespace Stage0
{
    partial class Program
    {
         static void Main(string[] args)
        {
            Welcome3529();
            Welcome5958();
            Console.ReadKey();
        }
        static partial void Welcome5958();
        private static void Welcome3529()
        {
            Console.Write("Enter your name: ");
            string nameUser = Console.ReadLine() ?? "";
            Console.WriteLine("{0}, welcome to my first console application", nameUser);
        }
    }
}