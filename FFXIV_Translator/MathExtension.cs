using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFXIV_Translator
{
    public static class MathExtension
    {
        public static float Lerp(float a, float b, float t)
        {
            if (t <= 0f)
                return a;
            if (t >= 1f)
                return b;

            return (int)(a + (b - a) * t);
        }

        public static int Distance(int a, int b)
        {
            return a > b ? a - b : a < b ? b - a : 0;
        }
    }
}
