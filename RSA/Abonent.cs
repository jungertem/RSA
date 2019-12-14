using System;
using System.Numerics;
using System.Text;

namespace RSA
{
    class Abonent
    {
        #region PRIVATE KEY PARTS
        public  BigInteger P { get; set; }
        public BigInteger Q { get; set; }
        private BigInteger D { get; set; }
        #endregion

        #region PUBLIC KEY PARTS
        public BigInteger N { get; private set; } // another part of the public key (modulus)
        public BigInteger Exponent { get; private set; } //part of the public key
        #endregion

        #region OTHER PARTS
        private BigInteger EulerFunctionResult { get; set; }
        public string Name { get; set; }

        private BigInteger ReceivedKey;
        #endregion

        public Abonent(int bitLengthPQ)
        {
            GeneratePrimeNumbers(bitLengthPQ);
            N = BigInteger.Multiply(P, Q);
            EulerFunctionResult = BigInteger.Multiply(P - 1, Q - 1);
            Exponent = (int) Math.Pow(2, 16) + 1;  
            D = BigIntegerExtensions.ModInverse(Exponent, EulerFunctionResult);

            //key not yet received
            ReceivedKey = BigInteger.MinusOne;
        }

        public Tuple<BigInteger, BigInteger, BigInteger> GetSecretKey()
        {
            var secretKey = new Tuple<BigInteger, BigInteger, BigInteger>(D, P, Q);
            return secretKey;
        }
        public Tuple<BigInteger, BigInteger> GetPublicKey()
        {
            var publicKey = new Tuple<BigInteger, BigInteger>(Exponent, N);
            return publicKey;
        }

        public BigInteger Encrypt(BigInteger message)
        {
            var encrypted = BigInteger.ModPow(message, Exponent, N);
            return encrypted;
        }
        public BigInteger Decrypt(BigInteger cipherText)
        {
            var decrypted = BigInteger.ModPow(cipherText, D, N);
            return decrypted;
        }

        public Tuple<BigInteger, BigInteger> Sign(BigInteger message)
        {
            var secretKeyS = BigInteger.ModPow(message, D, N);
            var signedMessage = new Tuple<BigInteger, BigInteger>(message, secretKeyS);

            return signedMessage;
        }
        public bool Verify(Tuple<BigInteger, BigInteger> signedMessage)
        {
            BigInteger S = signedMessage.Item2;
            BigInteger M = signedMessage.Item1;
            bool condition = (M == BigInteger.ModPow(S, Exponent, N));

            return condition;
        }

        #region PROTOCOL
        public void SendKey(Abonent sender)
        {

        }

        public void ReceiveKey()
        {

        }
        #endregion

        private void GeneratePrimeNumbers(int bitLength)
        {
            do
            {
                P = BigIntegerExtensions.NextBigInteger(bitLength);
            }
            while (!P.MillerRabinTest(1250));

            System.Threading.Thread.Sleep(1000);

            do
            {
                Q = BigIntegerExtensions.NextBigInteger(bitLength);
            }
            while (!Q.MillerRabinTest(1250));
        }
        public override string ToString()
        {
            StringBuilder information = new StringBuilder();

            information.AppendLine(string.Empty);
            information.AppendLine($"{new string('-', 100)}");
            information.AppendLine($"Abonent's name: <{Name}>");
            information.AppendLine($"P = {P.ToString("X")}");
            information.AppendLine($"Q = {Q.ToString("X")}");
            information.AppendLine($"N = {N.ToString("X")}");
            information.AppendLine($"D = {D.ToString("X")}");
            information.AppendLine($"Exponent = {Exponent.ToString("X")}");
            information.AppendLine($"P(N) = {EulerFunctionResult.ToString("X")}");
            information.AppendLine($"{new string('-', 100)}");

            return information.ToString();
        }

    }
}
