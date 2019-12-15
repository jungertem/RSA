using System;
using System.Numerics;

namespace RSA
{
    class CryptoSystem
    {
        public Abonent A { get; private set; }
        public Abonent B { get; private set; }

        public CryptoSystem(Abonent a, Abonent b)
        {
            A = a;
            B = b;
        }

        public void SendKey(Abonent sender, Abonent receiver, BigInteger key)
        {
            if (!CheckCondition())
            {
                throw new ArgumentException("p*q > p1*q1");
            }
            sender.SendKey(receiver, key);
        }
        private bool CheckCondition()
        {
            bool condition = A.P * A.Q < B.P * B.Q;

            return condition;
        }
    }
}
