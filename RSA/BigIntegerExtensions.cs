using System;
using System.Numerics;
using System.Linq;
using System.Security.Cryptography;

namespace RSA
{
    public static partial class BigIntegerExtensions
    {
        public static BigInteger ModInverse(this BigInteger a, BigInteger m)
        {
            if (m == 1) return 0;
            BigInteger m0 = m;
            (BigInteger x, BigInteger y) = (1, 0);

            while (a > 1)
            {
                BigInteger q = a / m;
                (a, m) = (m, a % m);
                (x, y) = (y, x - q * y);
            }
            return x < 0 ? x + m0 : x;
        }
        public static bool MillerRabinTest(this BigInteger n, int k)
        {
            // если n == 2 или n == 3 - эти числа простые, возвращаем true
            if (n == 2 || n == 3)
            {
                return true;
            }

            // если n < 2 или n четное - возвращаем false
            if (n < 2 || n % 2 == 0)
            {
                return false;
            }

            // представим n − 1 в виде (2^s)·t, где t нечётно, это можно сделать последовательным делением n - 1 на 2
            BigInteger t = n - 1;
            int s = 0;
            while (t % 2 == 0)
            {
                t /= 2;
                s += 1;
            }

            // повторить k раз
            for (int i = 0; i < k; i++)
            {
                // выберем случайное целое число a в отрезке [2, n − 2]
                //RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                Random rand = new Random();
                byte[] _a = new byte[n.ToByteArray().LongLength];

                BigInteger a;

                do
                {
                    rand.NextBytes(_a);
                    a = new BigInteger(_a);
                }
                while (a < 2 || a >= n - 2);

                // x ← a^t mod n, вычислим с помощью возведения в степень по модулю
                BigInteger x = BigInteger.ModPow(a, t, n);

                // если x == 1 или x == n − 1, то перейти на следующую итерацию цикла
                if (x == 1 || x == n - 1)
                    continue;

                // повторить s − 1 раз
                for (int r = 1; r < s; r++)
                {
                    // x ← x^2 mod n
                    x = BigInteger.ModPow(x, 2, n);

                    // если x == 1, то вернуть "составное"
                    if (x == 1)
                        return false;

                    // если x == n − 1, то перейти на следующую итерацию внешнего цикла
                    if (x == n - 1)
                        break;
                }

                if (x != n - 1)
                    return false;
            }

            // вернуть "вероятно простое"
            return true;
        }
        public static BigInteger NextBigInteger(int bitLength)
        {
            if (bitLength < 1)
            {
                return BigInteger.Zero;
            }

            int bytes = bitLength / 8;
            int bits = bitLength % 8;

            // Generates enough random bytes to cover our bits.
            byte[] bs = new byte[bytes + 1];
            (new Random()).NextBytes(bs);

            // Mask out the unnecessary bits.
            byte mask = (byte)(0xFF >> (8 - bits));
            bs[bs.Length - 1] &= mask;

            return new BigInteger(bs);
        }
        public static bool IsPseudoPrime(this BigInteger numberToCheck, BigInteger baseNumber)
        {
            bool firstCondition = BigInteger.GreatestCommonDivisor(numberToCheck, baseNumber) == 1;
            bool secondCondition = BigInteger.ModPow(numberToCheck, baseNumber - 1, baseNumber) == 1;

            return firstCondition & secondCondition;
        }
    }
   
}
