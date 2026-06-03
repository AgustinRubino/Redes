using UnityEngine;

public static class Vector2Extensions
{
    public static Vector2 Project(this Vector2 vector, Vector2 onNormal)
    {
        float num = Vector2.Dot(onNormal, onNormal);
        if (num < Mathf.Epsilon)
        {
            return Vector2.zero;
        }

        float num2 = Vector2.Dot(vector, onNormal);
        return new Vector2(onNormal.x * num2 / num, onNormal.y * num2 / num);
    }


    public static float AngleTo2(this Vector2 vect, Vector2 target)
    {
        var value = Vector2.Dot(vect, target) / (vect.magnitude * target.magnitude);
        return Mathf.Acos(value);
    }

    public static float AngleTo(this Vector2 vect, Vector2 target)
    {
        return Mathf.Atan2(target.y, target.x) - Mathf.Atan2(vect.y, vect.x);
    }
    public static Vector2 Clamp(this Vector2 vect, Vector2 min, Vector2 max)
    {
        return new Vector2(Mathf.Clamp(vect.x, min.x, max.x), Mathf.Clamp(vect.y, min.y, max.y));
    }

    public static Vector2 Scalar(this Vector2 vector, Vector2 scalarVector) => new Vector2(vector.x * scalarVector.x, vector.y * scalarVector.y);
    public static Vector2 Scalar(this Vector2 vector, float x, float y) => new Vector2(vector.x * x, vector.y * y);
    public static Vector2 Inverse(this Vector2 vector) => new Vector2(1 / vector.x, 1 / vector.y);

    public static Vector2 DirectionTo(this Vector2 v, Vector2 target, float scalar = 1) =>
        (target - v).normalized * scalar;

    public static float DistanceOf(this Vector2 v, Vector2 target) =>
        (target - v).magnitude;

    public static Vector2 Normal(this Vector2 v, bool clockwise = false)
    {
        return clockwise ? new(v.y, -v.x) : new(-v.y, v.x);
    }

    public static bool Between(this Vector2 vector, Vector2 v1, Vector2 v2)
    {
        if (!vector.x.Between(v1.x, v2.x)) return false;
        if (!vector.y.Between(v1.y, v2.y)) return false;
        return true;
    }

    // Convertions

    public static Vector2Int ToVectorInt(this Vector2 vect)
    {
        return new Vector2Int(
                Mathf.RoundToInt(vect.x),
                Mathf.RoundToInt(vect.y)
            );
    }
    public static Vector3 ToVector3(this Vector2 v, float? z = null)
    {
        return new Vector3(
            v.x,
            v.y,
            z: z.HasValue ? z.Value : 0
            );
    }
    public static Vector3Int ToVector3Int(this Vector2 v, int? z = null)
    {
        return new Vector3Int(
                Mathf.RoundToInt(v.x),
                Mathf.RoundToInt(v.y),
                z.HasValue ? z.Value : 0
            );
    }


    #region Tuples

    public static Vector2 ToVector(this (float x, float y) tuple) => new Vector2(tuple.x, tuple.y);
    public static void Deconstruct(this Vector2 vector, out float x, out float y)
    {
        x = vector.x;
        y = vector.y;
    }

    #endregion

    public static Vector2 With(this Vector2 v, float? x = null, float? y = null)
    {
        v.x = x.HasValue ? x.Value : v.x;
        v.y = y.HasValue ? y.Value : v.y;
        return v;
    }
}