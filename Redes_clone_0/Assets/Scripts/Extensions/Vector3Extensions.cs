using UnityEngine;

public static class Vector3Extensions
{
    public static Vector3 Scalar(this Vector3 vector, Vector3 scalarVector) 
        => new Vector3(vector.x * scalarVector.x, vector.y * scalarVector.y, vector.z * scalarVector.z);
    public static Vector3 Scalar(this Vector3 vector, float x, float y, float z) 
        => new Vector3(vector.x * x, vector.y * y, vector.z * z);

    public static Vector3 Inverse(this Vector3 vector) 
        => new Vector3(1 / vector.x, 1 / vector.y, 1 / vector.z);

    public static Vector3 Clamp(this Vector3 vect, Vector3 min, Vector3 max)
    {
        return new Vector3(
            Mathf.Clamp(vect.x, min.x, max.x),
            Mathf.Clamp(vect.y, min.y, max.y),
            Mathf.Clamp(vect.z, min.z, max.z));
    }

    public static Vector3 DirectionTo(this Vector3 v, Vector3 target, bool length = false, float scalar = 1) =>
        length ? (target - v) * scalar : (target - v).normalized * scalar;

    public static float DistanceOf(this Vector3 v, Vector3 target) =>
        (target - v).magnitude;

    public static bool Between(this Vector3 vector, Vector3 v1, Vector3 v2)
    {
        if (!vector.x.Between(v1.x, v2.x)) return false;
        if (!vector.y.Between(v1.y, v2.y)) return false;
        if (!vector.z.Between(v1.z, v2.z)) return false;
        return true;
    }

    public static Vector3 RotateY(this Vector3 v, float angle)
    {
        var a = Mathf.Atan2(v.z, v.x);
        return new Vector3(Mathf.Cos(a + angle), v.y, Mathf.Sin(a + angle)) * v.magnitude;
    }

    public static Vector2 ToVector2(this Vector3 vector) => new Vector2(vector.x, vector.y);
    public static Vector3Int ToVector3Int(this Vector3 v)
    {
        return new Vector3Int(
                Mathf.RoundToInt(v.x),
                Mathf.RoundToInt(v.y),
                Mathf.RoundToInt(v.z)
            );
    }
    public static Color ToColor(this Vector3 vector, float a = 1, bool clamp = false)
    {
        if (clamp) return new Color(vector.x, vector.y, vector.z, a);

        return new Color(vector.x / 255, vector.y / 255, vector.z / 255, a);
    }

    #region Tuple
    public static Vector3 ToVector(this (float x, float y, float z) tuple) => new Vector3(tuple.x, tuple.y, tuple.z);
    public static void Deconstruct(this Vector3 vector, out float x, out float y, out float z)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }
    #endregion

    public static Vector3 With(this Vector3 v, float? x = null, float? y = null, float? z = null)
    {
        v.x = x.HasValue ? x.Value : v.x;
        v.y = y.HasValue ? y.Value : v.y;
        v.z = z.HasValue ? z.Value : v.z;
        return v;
    }

    public static Vector3 VectorRight(this Vector3 vector)
    {
        return vector.With(x: vector.z, z: -vector.x);
    }
    public static Vector3 VectorUp(this Vector3 vector)
    {
        return vector.With(y: vector.z, z: -vector.y);
    }
}
