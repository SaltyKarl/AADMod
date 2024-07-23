using UnityEngine;

namespace AADMod;

public static class VectorHelper
{
    public static Vector3 GetVector3_By_AngleFlat(Vector3 Center, float Range, float Angle)
    {
        Angle %= 360f;
        if (Angle < 0f)
        {
            Angle += 360f;
        }

        float radians = Angle * Mathf.Deg2Rad;
        float x2 = Center.x - Range * Mathf.Sin(radians);
        float z2 = Center.z - Range * Mathf.Cos(radians);
        return new Vector3(x2, Center.y, z2);
    }
}
