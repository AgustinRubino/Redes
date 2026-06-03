using UnityEngine;

public static class TransformExtensions
{
    /// <summary>
    /// Copies the values from the source Transform to this transform
    /// </summary>
    /// <param Name="t">this transform</param>
    /// <param Name="source">the transform source</param>
    /// <returns></returns>
    public static Transform Copy(this Transform t, Transform source)
    {
        t.position = source.position;
        t.rotation = source.rotation;
        t.localScale = source.localScale;
        return t;
    }

    // Global Position

    /// <summary>
    /// Returns The X position of the transform
    /// </summary>
    /// <param Name="t"></param>
    /// <returns></returns>
    public static float PositionX(this Transform t)
    {
        return t.position.x;
    }

    /// <summary>
    /// Sets the X Position of the transform, and returns the transform itself
    /// </summary>
    /// <param Name="t"></param>
    /// <param Name="x">The target X position</param>
    /// <returns></returns>
    public static Transform PositionX(this Transform t, float x)
    {
        t.position = t.position.With(x: x);
        return t;
    }

    /// <summary>
    /// Returns The Y position of the transform
    /// </summary>
    /// <param Name="t"></param>
    /// <returns></returns>
    public static float PositionY(this Transform t)
    {
        return t.position.y;
    }

    /// <summary>
    /// Sets the Y Position of the transform, and returns the transform itself
    /// </summary>
    /// <param Name="t"></param>
    /// <param Name="y">The target Y position</param>
    /// <returns></returns>
    public static Transform PositionY(this Transform t, float y)
    {
        t.position = t.position.With(y: y);
        return t;
    }

    /// <summary>
    /// Returns The Z position of the transform
    /// </summary>
    /// <param Name="t"></param>
    /// <returns></returns>
    public static float PositionZ(this Transform t)
    {
        return t.position.z;
    }

    /// <summary>
    /// Sets the Z Position of the transform, and returns the transform itself
    /// </summary>
    /// <param Name="t"></param>
    /// <param Name="z">the target Z position</param>
    /// <returns></returns>
    public static Transform PositionZ(this Transform t, float z)
    {
        t.position = t.position.With(z: z);
        return t;
    }

    /// <summary>
    /// Returns a Vector2 with the X and Y position of the transform
    /// </summary>
    /// <param Name="t"></param>
    /// <returns></returns>
    public static Vector2 Position2D(this Transform t)
    {
        return t.position;
    }

    /// <summary>
    /// Sets the X and Y Position of the transform, and returns the transform itself
    /// </summary>
    /// <param Name="t"></param>
    /// <param Name="position">The vector with the X and Y values</param>
    /// <returns></returns>
    public static Transform Position2D(this Transform t, Vector2 position)
    {
        t.position = position;
        return t;
    }

    /// <summary>
    /// Sets the X and Y Position of the transform, and returns the transform itself
    /// </summary>
    /// <param Name="t"></param>
    /// <param Name="x">The target X position</param>
    /// <param Name="y">The target Y position</param>
    /// <returns></returns>
    public static Transform Position2D(this Transform t, float x, float y)
    {
        t.position = new Vector3(x, y, t.PositionZ());
        return t;
    }

    public static Transform SetPosition(this Transform t, float? x = null, float? y = null, float? z = null)
    {
        if (x.HasValue) t.PositionX(x.Value);
        if (y.HasValue) t.PositionY(y.Value);
        if (z.HasValue) t.PositionZ(z.Value);
        return t;
    }
    public static Transform SetPosition(this Transform t, Vector3 position)
    {
        t.position = position;
        return t;
    }

    public static Transform AddPosition(this Transform t, Vector3 position)
    {
        t.position += position;
        return t;
    }


    public static float LocalPosX(this Transform t)
    {
        return t.localPosition.x;
    }
    public static Transform LocalPosX(this Transform t, float x)
    {
        t.localPosition = t.localPosition.With(x: x);
        return t;
    }
    public static float LocalPosY(this Transform t)
    {
        return t.localPosition.y;
    }
    public static Transform LocalPosY(this Transform t, float y)
    {
        t.localPosition = t.localPosition.With(y: y);
        return t;
    }
    public static float LocalPosZ(this Transform t)
    {
        return t.localPosition.z;
    }
    public static Transform LocalPosZ(this Transform t, float z)
    {
        t.localPosition = t.localPosition.With(z: z);
        return t;
    }
    public static Vector2 LocalPosition2D(this Transform t)
    {
        return t.localPosition;
    }
    public static Transform LocalPosition2D(this Transform t, Vector2 position)
    {
        t.localPosition = position;
        return t;
    }
    public static Transform LocalPosition2D(this Transform t, float x, float y)
    {
        t.localPosition = new Vector3(x, y, t.PositionZ());
        return t;
    }

    // Rotation
    public static float Rotation2D(this Transform t)
    {
        return t.eulerAngles.z;
    }

    public static Transform Rotation2D(this Transform t, float angle)
    {
        t.rotation = Quaternion.Euler(0, 0, angle);
        return t;
    }

    // Scale
    public static float ScaleX(this Transform t)
    {
        return t.localScale.x;
    }
    public static Transform ScaleX(this Transform t, float x)
    {
        t.localScale = t.localScale.With(x);
        return t;
    }

    public static float ScaleY(this Transform t)
    {
        return t.localScale.y;
    }
    public static Transform ScaleY(this Transform t, float y)
    {
        t.localScale = t.localScale.With(y: y);
        return t;
    }

    public static float ScaleZ(this Transform t)
    {
        return t.localScale.z;
    }
    public static Transform ScaleZ(this Transform t, float z)
    {
        t.localScale = t.localScale.With(z: z);
        return t;
    }


}