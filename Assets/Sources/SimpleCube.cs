using DG.Tweening;
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleCube : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;

    private Transform _transform;
    private Sequence _idleAnimation;
    private Sequence _dissolveAnimation;
    private SimpleCubeAnimationSpawner _animationSpawner;
    private Rigidbody _rigidbody;

    public event Action<SimpleCube> Dissolved;

    public Material Material => _meshRenderer.material;

    private void Awake()
    {
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;
    }

    private void OnDisable()
    {
        _dissolveAnimation.Kill();
    }

    private void Start()
    {
        if (_idleAnimation == null)
            _idleAnimation = _animationSpawner.GetIdleAnimation(_transform);

        _idleAnimation.Restart();
    }

    public void SetMaterial(Material material) => _meshRenderer.material = material;
    public void SetAnimationSpawner(SimpleCubeAnimationSpawner animationSpawner) => _animationSpawner = animationSpawner;

    public void DropDown()
    {
        _idleAnimation.Pause();
        _idleAnimation.Kill();
        _rigidbody.isKinematic = false;
    }

    public void Dissolve(Vector3 destination)
    {
        _rigidbody.isKinematic = true;
        _dissolveAnimation = _animationSpawner.GetDissolveAnimation(_transform, destination);
        _dissolveAnimation.Play().OnComplete(() => Dissolved?.Invoke(this));
    }
}