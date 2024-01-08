using Raylib_CsLo;
using System.Numerics;

namespace HopeEngine.ParticleGame;

public static class ParticleUtils
{
    public static Color GetRandomColor()
    {
        return new Color(Raylib.GetRandomValue(0, 255), Raylib.GetRandomValue(0, 255), Raylib.GetRandomValue(0, 255), 255);
    }

    public static Color LerpColor(Color start, Color end, float t)
    {
        t = Math.Max(0, Math.Min(1, t)); // Ensure t is in the [0, 1] range

        byte r = (byte)Math.Round(start.r + t * (end.r - start.r));
        byte g = (byte)Math.Round(start.g + t * (end.g - start.g));
        byte b = (byte)Math.Round(start.b + t * (end.b - start.b));
        byte a = (byte)Math.Round(start.a + t * (end.a - start.a));

        return new Color(r, g, b, a);
    }

    public static Vector2 GetRandomPointInCircle(float radius = 1f)
    {
        double angleInRadians = Raylib.GetRandomValue(0, 360) * Math.PI / 180.0;
        double randomDistance = Math.Sqrt(Raylib.GetRandomValue(0, 1000000)) / 1000.0 * radius;

        float offsetX = (float)(randomDistance * Math.Cos(angleInRadians));
        float offsetY = (float)(randomDistance * Math.Sin(angleInRadians));

        return new Vector2(offsetX, offsetY);
    }

    public static Vector2 GetAttraction(Vector2 pos, float mass, Vector2 oPos, float oMass, float gForce = 10f, float pVal = 1.5f, float extraF = 0f)
    {
        var gaDir = pos - oPos;
        var gaDist = gaDir.Length();

        return Vector2.Normalize(gaDir) * gForce * ((mass * oMass) / (gaDist * pVal) + extraF);
        //return Vector2.Normalize(gaDir) * gForce * ((mass * oMass) / MathF.Pow(gaDist, pVal) + extraF);
    }
}
