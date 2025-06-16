
public class MathUtils
{
    public static float NormalizeAngle(float angle)
    {
        return (angle % 360 + 360) % 360;
    }
}
