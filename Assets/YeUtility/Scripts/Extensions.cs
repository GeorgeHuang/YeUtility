using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public static class Extensions
{
    public static void ForEach<T>(this IEnumerable<T> sequence, Action<int, T> action)
    {
        // argument null checking omitted
        int i = 0;
        foreach (T item in sequence)
        {
            action(i, item);
            i++;
        }
    }

    public static decimal Map(this decimal value, decimal fromSource, decimal toSource, decimal fromTarget,
        decimal toTarget)
    {
        return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
    }

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public static byte[] ToByte(this string input)
    {
        return Encoding.UTF8.GetBytes(input);
    }

    public static string ToSting(this byte[] input)
    {
        return Encoding.UTF8.GetString(input);
    }

    public static string INTToStr(this int value, int digits)
    {
        var f = "{0:";
        for (var i = 0; i < digits; ++i) f += "0";
        f += "}";
        return string.Format(f, value);
    }

    public static void Swap(this IList l, int i, int j)
    {
        (l[i], l[j]) = (l[j], l[i]);
    }
}