using System;
using System.Numerics;
using System.Text;

namespace RSA
{
    class Abonent
    {
        #region PRIVATE KEY PARTS
        public BigInteger P { get; set; }
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
        private BigInteger KeyFromSender;
        private Tuple<BigInteger, BigInteger> ReceivedKeyToVerify;
        #endregion

        public Abonent(int bitLengthPQ)
        {
            GeneratePrimeNumbers(bitLengthPQ);
            N = BigInteger.Multiply(P, Q);
            EulerFunctionResult = BigInteger.Multiply(P - 1, Q - 1);
            Exponent = (int)Math.Pow(2, 16) + 1;
            D = BigIntegerExtensions.ModInverse(Exponent, EulerFunctionResult);
            ReceivedKeyToVerify = null;
        }
        public BigInteger Encrypt(Tuple<BigInteger,BigInteger> publicKey, BigInteger message)
        {
            var exponent = publicKey.Item2;
            var modulus = publicKey.Item1;

            var encrypted = BigInteger.ModPow(message, exponent, modulus);
            return encrypted;
        }
        public BigInteger Decrypt(BigInteger cipherText)
        {
            var decrypted = BigInteger.ModPow(cipherText, D, N);
            return decrypted;
        }

        public Tuple<BigInteger, BigInteger> Sign(BigInteger message)
        {
            var s = BigInteger.ModPow(message, D, N);
            var signedMessage = new Tuple<BigInteger, BigInteger>(message, s);

            return signedMessage;
        }
        public bool Verify(Abonent sender, Tuple<BigInteger, BigInteger> signedMessage)
        {
            BigInteger M = signedMessage.Item1;
            BigInteger S = signedMessage.Item2;

            return (M == BigInteger.ModPow(S, sender.Exponent, sender.N));
        }

        #region PROTOCOL
        public void SendKey(Abonent receiver, BigInteger key)
        {
            if (key > N)
            {
                throw new ArgumentOutOfRangeException("Key can't be greater then N");
            }

            var k1 = BigInteger.ModPow(key, receiver.Exponent, receiver.N);
            var s = BigInteger.ModPow(key, this.D, this.N);
            var s1 = BigInteger.ModPow(s, receiver.Exponent, receiver.N);

            receiver.ReceivedKeyToVerify = new Tuple<BigInteger, BigInteger>(k1, s1);
            receiver.ReceiveKey(this);
        }

        public void ReceiveKey(Abonent sender)
        {
            if (ReceivedKeyToVerify == null)
            {
                throw new ArgumentNullException();
            }

            var k1 = ReceivedKeyToVerify.Item1;
            var s1 = ReceivedKeyToVerify.Item2;

            var k = BigInteger.ModPow(k1, this.D, this.N);
            var s = BigInteger.ModPow(s1, this.D, this.N);

            if (!Verify(sender, new Tuple<BigInteger, BigInteger>(k, s)))
            {
                ReceivedKeyToVerify = null;
                throw new ArgumentException("Wrong signification!");
            }

            KeyFromSender = k;
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
            information.AppendLine($"Received key = {KeyFromSender.ToString("X")}");
            information.AppendLine($"{new string('-', 100)}");

            return information.ToString();
        }

    }
}
