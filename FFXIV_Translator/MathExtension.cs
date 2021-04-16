namespace FFXIV_Translator
{
    public static class MathExtension
    {
        public static float Lerp(float a, float b, float t)
        {
            return t <= 0f ? a : t >= 1f ? b : (int)(a + (b - a) * t);
        }

        public static int Distance(int a, int b)
        {
            return a > b ? a - b : a < b ? b - a : 0;
        }
    }
}
