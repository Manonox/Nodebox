namespace Nodebox;

public static class AnglesExtensions {
    public static Angles Negate(this Angles v) => new(
        -v.pitch,
        -v.yaw,
        -v.roll
    );
}
