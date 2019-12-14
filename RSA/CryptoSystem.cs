using System;

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

        public void CheckCondition()
        {
            bool condition = A.P * A.Q < B.P * B.Q;

            Console.WriteLine(condition);
        }
    }
}
