using UnityEngine;

public interface IReadonlyCube
{
    public int Index { get; }
    public Material Material { get; }

    public void EnableRendering();
    public void DisableRendering();
    public void Highlight(Material material);
    public void StopHighlight();
}