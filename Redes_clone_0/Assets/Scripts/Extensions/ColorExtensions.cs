using UnityEngine;

public static class ColorExtensions
{
    public static Color SetAlpha(this Color color, float alpha)
    {
        color.a = Mathf.Clamp(alpha, 0, 1);
        return color;
    }

    /// <summary>
    /// Returns a new color equal
    /// </summary>
    /// <param Name="c"></param>
    /// <param Name="r"></param>
    /// <param Name="g"></param>
    /// <param Name="b"></param>
    /// <param Name="a"></param>
    /// <returns></returns>
    public static Color Set(this Color c, int r, int g, int b, float a = 1)
    {
        return c.With((float)r / 255, (float)g / 255, (float)b / 255, a);
    }


    /// <summary>
    /// Returns a new Color equal as the original, but with the modifications of the parameters
    /// </summary>
    /// <param Name="c">original color</param>
    /// <param Name="r">red parameter</param>
    /// <param Name="g">green parameter</param>
    /// <param Name="b">blue parameter</param>
    /// <param Name="a">alpha parameter</param>
    /// <returns></returns>
    public static Color With(this Color c, float? r = null, float? g = null, float? b = null, float? a = null)
    {
        return new(r.HasValue ? r.Value : c.r,
                    g.HasValue ? g.Value : c.g,
                    b.HasValue ? b.Value : c.b,
                    a.HasValue ? a.Value : c.a);
    }


}