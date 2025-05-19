using DG.Tweening;
using UnityEngine;

public class SimpleCube : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;

    private Transform _transform;
    private Sequence _animation;

    private void Awake()
    {
        _transform = transform;
        _animation = UserUtils.GetAnimation(_transform);
    }

    private void Start()
    {
        _animation.Restart();
    }

    public void SetMaterial(Material material) => _meshRenderer.material = material;
}