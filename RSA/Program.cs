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
            Console.WriteLine(a);
            //Abonent b = new Abonent(bitLengthPQ: 256 + 128) { Name = "Bob" };

            //Console.WriteLine(b);

            //#region ENCRYPTION
            //BigInteger serverModulus = BigInteger.Parse("012F740BA9F755D9FC41C703BDB56A4333BBD63E0888C16E0AE5E6265120FEE997", NumberStyles.AllowHexSpecifier);
            //BigInteger serverExponent = (int)Math.Pow(2, 16) + 1;

            //var publicKey = new Tuple<BigInteger, BigInteger>(serverModulus, serverExponent);
            //var messageToEncrypt = BigInteger.Parse("0BBA67B", NumberStyles.AllowHexSpecifier);

            //var encryptedMessage = a.Encrypt(publicKey, messageToEncrypt);
            //Console.WriteLine("Encrypted for server " + encryptedMessage.ToString("X"));
            //#endregion

            //#region DECRYPTION 
            //Console.WriteLine("Enter encrytped message from server: ");
            //var bytes = Console.ReadLine();

            //var messageToDecrypt = BigInteger.Parse("0" + bytes, NumberStyles.AllowHexSpecifier);
            //var decryptedMessage = a.Decrypt(messageToDecrypt);
            //Console.WriteLine("Decrypted message on Abonent a " + decryptedMessage.ToString("X"));
            //#endregion

            //#region VERIFICATION

            //var M = BigInteger.Parse("0ffffb111", NumberStyles.AllowHexSpecifier);
            //var S = BigInteger.Parse("0352299B5379259F213D1025A354457D81ABAF2D6A71AE77BCD1458EC3D9323E0", NumberStyles.AllowHexSpecifier);
            //var signedMessage = new Tuple<BigInteger, BigInteger>(M, S);

            //bool verif = a.Verify(publicKey, signedMessage);
            //Console.WriteLine("VERIFICATION RESULT " + verif);

            //#endregion

            //#region SIGNATURE
            //var messageToSign = BigInteger.Parse("0FFF123B", NumberStyles.AllowHexSpecifier);
            //var signedMessageForServer = a.Sign(messageToSign);

            //Console.WriteLine("SIGNED FOR SERVER = ");
            //Console.WriteLine("M = " + signedMessageForServer.Item1.ToString("X"));
            //Console.WriteLine("S = " + signedMessageForServer.Item2.ToString("X"));

            //#endregion

            //CryptoSystem rsa = new CryptoSystem(a, b);
            //rsa.SendKey(a, b, 999);

            //Console.WriteLine(a);
            //Console.WriteLine(b);

            BigInteger serverModulus = BigInteger.Parse("09EEDBBB30810E2BD60FCBB9B37B7BCE698B8FF3AB035F912552D7E61B12B5A2D", NumberStyles.AllowHexSpecifier);
            BigInteger serverExponent = (int)Math.Pow(2, 16) + 1;

            var key = BigInteger.Parse("0FFFE", NumberStyles.AllowHexSpecifier);
            var publicKey = new Tuple<BigInteger, BigInteger>(serverModulus, serverExponent);
            var keyToSend = a.SendKey(publicKey, key);

            Console.WriteLine("K1 = " + keyToSend.Item1.ToString("X"));
            Console.WriteLine("S1 = " + keyToSend.Item2.ToString("X"));


            //-----------------------------------------------------------
            Console.WriteLine("Enter key : ");
            string val = Console.ReadLine();

            Console.WriteLine("Enter sig: ");
            string val1 = Console.ReadLine();

            var receivedKey = BigInteger.Parse("0" + val, NumberStyles.AllowHexSpecifier);
            var receivedSignature = BigInteger.Parse("0" + val1, NumberStyles.AllowHexSpecifier);
            var receivedFinalKey = a.ReceiveKey(publicKey, new Tuple<BigInteger, BigInteger>(receivedKey, receivedSignature));
            Console.WriteLine(receivedFinalKey.ToString("X"));


            Console.ReadKey();
        }
    }
}
