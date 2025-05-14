using UnityEngine;

public static class UserUtils 
{
    public const int ImageResolution = 32;
    public const int ColorizerBarCount = 5;
    public const int MinRotateSpeed = 10;
    public const int MaxRotateSpeed = 50;

    public const float Half = 2;
    public const float HalfUnit = 0.5f;
    public const float TimeForShow = 5f;

    private const float MinAlfa = 0.1f;

    private static FlipType[] s_flipTypes =
    {
        FlipType.XFlip,
        FlipType.YFlip,
        FlipType.ZFlip,
    };

    public static bool IsBlack(Color color) 
    {
        return Mathf.Approximately(color.r, 0)
            && Mathf.Approximately(color.g, 0)
            && Mathf.Approximately(color.b, 0);
    }

    public static bool IsTransparent(Color color) => color.a < MinAlfa;

    public static FlipType GetRandomFlipType()
    {
        int random = Random.Range(0, s_flipTypes.Length);
        return s_flipTypes[random];
    }
}