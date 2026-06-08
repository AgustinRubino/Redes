using System;
using UnityEngine;

public static class MathExtensions
{
    public static bool Between<T>(this IComparable<T> value, T min, T max, bool exclusive = false)
    {
        if (exclusive)
            return value.CompareTo(min) > 0 && value.CompareTo(max) < 0;
        else
            return value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0;
    }

    // float
    public static int Floor(this float number)
    {
        return Mathf.FloorToInt(number);
    }

    public static int CeilToInt(this float number)
    {
        return Mathf.CeilToInt(number);
    }

    public static int RoundToInt(this float number)
    {
        return Mathf.RoundToInt(number);
    }
    public static float Fract(this float number)
    {
        return number - number.Floor();
    }

    // int
    public static int Pow(this int number, int pow)
    {
        return number switch
        {
            <= -1 => 0,
            0 => 1,
            1 => number,
            2 => number * number,
            3 => number * number * number,
            _ => Mathf.Pow(number, pow).Floor()
        };
    }
    public static int Factorial(this int number)
    {
        if (number <= 1) return number;

        return number * number.Factorial();
    }

    public static Vector3 SmoothStep(this  Vector3 start, Vector3 end, float t)
    {
        return new Vector3(Mathf.SmoothStep(start.x, end.x, t), Mathf.SmoothStep(start.y, end.y, t), Mathf.SmoothStep(start.z, end.z, t));
    }
}