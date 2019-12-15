using System;
using System.Threading;
using System.Numerics;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Globalization;

namespace RSA
{
    class Program
    {
        static void Main(string[] args)
        {
            Abonent a = new Abonent(bitLengthPQ: 256) { Name = "Alice" };
            Abonent b = new Abonent(bitLengthPQ: 256 + 128) { Name = "Bob" };

            Console.WriteLine(a);
            Console.WriteLine(b);

            #region ENCRYPTION
            BigInteger serverModulus = BigInteger.Parse("0BBFD5300BE2518E4FA6FD3A73946F3EA610E0C3C77FE9B2CC0DC517902CBD1A9", NumberStyles.AllowHexSpecifier);
            BigInteger serverExponent = (int)Math.Pow(2, 16) + 1;

            var publicKey = new Tuple<BigInteger, BigInteger>(serverModulus, serverExponent);
            var messageToEncrypt = BigInteger.Parse("0ABCD", NumberStyles.AllowHexSpecifier);

            var encryptedMessage = a.Encrypt(publicKey, messageToEncrypt);
            Console.WriteLine("Encrypted for server " + encryptedMessage.ToString("X"));
            #endregion


            #region DECRYPTION 
            Console.WriteLine("Enter encrytped message from server: ");
            var bytes = Console.ReadLine();

            var messageToDecrypt = BigInteger.Parse("0" + bytes, NumberStyles.AllowHexSpecifier);
            var decryptedMessage = a.Decrypt(messageToDecrypt);
            Console.WriteLine("Decrypted message on Abonent a " + decryptedMessage.ToString("X"));
            #endregion

            CryptoSystem rsa = new CryptoSystem(a, b);
            rsa.SendKey(a, b, 999);

            Console.WriteLine(a);
            Console.WriteLine(b);


            Console.ReadKey();
        }
    }
}
