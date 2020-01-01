namespace MoonTools.Core.Bonk
{
    public static class LongHelper
    {
        public static long MakeLong(int left, int right)
        {
            return ((long)left << 32) | ((uint)right);
        }
    }
}
