namespace Nodebox;

public static class MathExtensions {
    // What the actual fuck

    public static int Pow(this int x, int exponent) => (int)Math.Pow(x, exponent);
    public static long Pow(this long x, int exponent) => (long)Math.Pow(x, exponent);
    
    // public static int Pow(this int x, int exponent) => (int)System.Numerics.BigInteger.Pow(x, exponent);
    // public static long Pow(this long x, int exponent) => (long)System.Numerics.BigInteger.Pow(x, exponent);

    public static double LerpTo(this double from, double to, double frac, bool clamp = true)
    {
        if (clamp)
        {
            frac = frac.Clamp(0.0, 1.0);
        }

        return from + frac * (to - from);
    }
}
