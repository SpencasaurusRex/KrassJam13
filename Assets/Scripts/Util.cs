using UnityEngine;

public static class Util
{
    public static float Remap(float startA, float startB, float endA, float endB, float value)
    {
        return Mathf.Lerp(endA, endB, Mathf.InverseLerp(startA, startB, value));
    }
    
    public static float RemapUnclamped(float startA, float startB, float endA, float endB, float value)
    {
        return Mathf.LerpUnclamped(endA, endB, InverseLerpUnclamped(startA, startB, value));
    }

    public static Color Lerp(Color a, Color b, float t)
    {
        return (1f - t) * a + t * b;
    }

    public static float InverseLerpUnclamped(float a, float b, float value)
    {
        return (value - a) / (b - a);
    }
}