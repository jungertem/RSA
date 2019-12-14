using System;
using System.Threading;
using System.Numerics;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace RSA
{
    class Program
    {
        static void Main(string[] args)
        {
            Abonent a = new Abonent(bitLengthPQ: 256) { Name = "Alice" };
            Abonent b = new Abonent(bitLengthPQ: 256 + 128) { Name = "Bob" };

            CryptoSystem rsa = new CryptoSystem(a, b);
            rsa.SendKey(a, b, 999);

            Console.WriteLine(a);
            Console.WriteLine(b);


            Console.ReadKey();
        }
    }
}
