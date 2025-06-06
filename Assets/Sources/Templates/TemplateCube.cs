using UnityEngine;

[RequireComponent (typeof(BoxCollider))]
public class TemplateCube : MonoBehaviour, IReadonlyTemplateCube
{
    [SerializeField] private MeshRenderer _meshRenderer;

    private bool _isInitiated;
    private bool _isColored;
    private Transform _transform;

    public bool IsMarked { get; private set; }
    public CubeType Type { get; private set; }
    public int Index { get; private set; }
    public Material Material { get; private set; }
    public Vector3 Position 
    {
        get 
        {
            if(_transform != null)
                return _transform.position;

            return transform.position;
        }
    }

    public void Init(CubeType type, int index)
    {
        if (_isInitiated)
            return;

        Type = type;
        Index = index;
        Material = _meshRenderer.material;
        _isInitiated = true;
        _transform = transform;
    }

    public void SetColored(Material material)
    {
        if (_isColored)
            return;

        _isColored = true;
        ApplyMaterial(material);
        EnableRendering();
    }

    public void DisableRendering()
    {
        _meshRenderer.material = Material;

        if (_isColored)
            return;

        _meshRenderer.enabled = false;
    }

    public void EnableRendering() => _meshRenderer.enabled = true;

    public void Highlight(Material material)
    {
        _meshRenderer.material = material;
    }

    public void StopHighlight()
    {
        _meshRenderer.material = Material;

        if (Type == CubeType.In)
            DisableRendering();
    }

    public void Mark() => IsMarked = true;

    private void ApplyMaterial(Material material)
    {
        _meshRenderer.material = material;
        Material = material;
    }
}