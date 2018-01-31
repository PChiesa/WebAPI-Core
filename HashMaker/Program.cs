using System;
using WebAPI.Services;

namespace HashMaker
{
    class Program
    {
        static HashStringService hash = new HashStringService();

        static void Main(string[] args)
        {
            Console.WriteLine("Type string you want to hash:");
            var typedString = Console.ReadLine();
            Console.WriteLine($"Typed string: {typedString}");

            var hashedString = hash.Hash(typedString);

            Console.WriteLine($"Hashed string: {hashedString}");

            Console.WriteLine("Type any key to exit");
            Console.ReadKey();
        }
    }
}
