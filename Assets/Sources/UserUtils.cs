using UnityEngine;

public static class UserUtils 
{
    public const int ImageResolution = 32;

    public const float Half = 2;
    public const float HalfUnit = 0.5f;
    public const float TimeForShow = 5f;

    private const float MinAlfa = 0.1f;

    public static bool IsBlack(Color color) 
    {
        return Mathf.Approximately(color.r, 0)
            && Mathf.Approximately(color.g, 0)
            && Mathf.Approximately(color.b, 0);
    }

    public static bool IsTransparent(Color color) => color.a < MinAlfa;
}