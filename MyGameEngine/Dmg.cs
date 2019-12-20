using System;

namespace MyGameEngine
{
    public class Dmg
    {
        private (int, int) val;

        public Dmg((int, int) a)
        {
            val = a;
        }

        public Dmg(Dmg x)
        {
            val = x.val;
        }

        public void Add(int x)
        {
            val = (val.Item1 + x, val.Item2 + x);
        }

        public void Mul(double x)
        {
            val = (Convert.ToInt32(val.Item1 * x), Convert.ToInt32(val.Item2 * x));
        }

        public int Min => val.Item1;

        public int Max => val.Item2;
    }
}