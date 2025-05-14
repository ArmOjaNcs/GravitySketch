using UnityEngine;

public interface IReadonlyTemplateCube
{
    public bool IsMarked {  get; }
    public CubeType Type { get; }
    public int Index { get; }
    public Material Material { get; }
    public Vector3 Position { get; }

    public void SetColored(Material material);
    public void EnableRendering();
    public void DisableRendering();
    public void Highlight(Material material);
    public void StopHighlight();
    public void Mark();
}