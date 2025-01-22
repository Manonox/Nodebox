namespace Nodebox.Util;

public static class MathOmega {
    public static float Pow(this float x, float exponent) => (float)MathF.Pow(x, exponent);
    public static double Pow(this double x, double exponent) => (double)Math.Pow(x, exponent);


    // What the actual fuck
    public static int Pow(this int x, int exponent) => (int)Math.Pow(x, exponent);
    public static long Pow(this long x, int exponent) => (long)Math.Pow(x, exponent);
    
    // public static int Pow(this int x, int exponent) => (int)System.Numerics.BigInteger.Pow(x, exponent);
    // public static long Pow(this long x, int exponent) => (long)System.Numerics.BigInteger.Pow(x, exponent);

    // Ok this ones stupid, but it's fine
    public static byte Pow(this byte x, int exponent) => (byte)(long)Math.Pow(x, exponent);

    public static double LerpTo(this double from, double to, double frac, bool clamp = true)
    {
        if (clamp)
        {
            frac = frac.Clamp(0.0, 1.0);
        }

        return from + frac * (to - from);
    }
    
    public static Color LerpTo(this Color from, Color to, float frac, bool clamp = true) => new(
        from.r.LerpTo(to.r, frac, clamp),
        from.g.LerpTo(to.g, frac, clamp),
        from.b.LerpTo(to.b, frac, clamp),
        from.a.LerpTo(to.a, frac, clamp)
    );

    
    public static double UnsignedMod(this double a, double b) {
        return a - b * Math.Floor(a / b);
    }

    public static double DeltaRadians(double from, double to) {
        double num = UnsignedMod(to - from, Math.PI * 2.0);
        if (!(num >= Math.PI))
        {
            return num;
        }

        return num - Math.PI * 2f;
    }

    public static double DeltaDegrees(double from, double to) {
        double num = UnsignedMod(to - from, 360f);
        if (!(num >= 180.0))
        {
            return num;
        }

        return num - 360.0;
    }

    //
    // Summary:
    //     Convert degrees to radians.
    //
    //     180 degrees is System.Math.PI (roughly 3.14) radians, etc.
    //
    // Parameters:
    //   deg:
    //     A value in degrees to convert.
    //
    // Returns:
    //     The given value converted to radians.
    public static double DegreeToRadian(this double deg) {
        return deg * Math.PI / 180.0;
    }

    //
    // Summary:
    //     Convert radians to degrees.
    //
    //     180 degrees is System.Math.PI (roughly 3.14) radians, etc.
    //
    // Parameters:
    //   rad:
    //     A value in radians to convert.
    //
    // Returns:
    //     The given value converted to degrees.
    public static double RadianToDegree(this double rad) {
        return rad * 180.0 / Math.PI;
    }
}
