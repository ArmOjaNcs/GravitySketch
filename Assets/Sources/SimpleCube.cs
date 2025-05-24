using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleCube : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;

    private Transform _transform;
    private Sequence _idleAnimation;
    private Tween _dissolveAnimation;
    private SimpleCubeAnimationSpawner _animationSpawner;
    private Rigidbody _rigidbody;
    private Vector3 _defaultScale;

    public event Action<SimpleCube> Dissolved;

    public Material Material => _meshRenderer.material;

    private void Awake()
    {
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;
        _defaultScale = _transform.localScale;
    }

    private void OnEnable()
    {
        _transform.localScale = _defaultScale;
    }

    private void Start()
    {
        _idleAnimation = _animationSpawner.GetIdleAnimation(_transform);
        _dissolveAnimation = _animationSpawner.GetDissolveAnimation(_transform);
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

    public void Dissolve(Transform hole)
    {
        _rigidbody.isKinematic = true;
        _dissolveAnimation.Restart();
        StartCoroutine(FallingRoutine(hole, _dissolveAnimation.Duration()));
    }

    private IEnumerator FallingRoutine(Transform hole, float duration)
    {
        float elapsedTime = 0;
        Debug.Log("OnEnter" + hole.position + "duration" + duration);
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedPosition = elapsedTime / duration;
            _transform.position = Vector3.Lerp(_transform.position, hole.position, normalizedPosition);

            yield return null;
        }
        Debug.Log("OnExit" + hole.position);
        _transform.position = hole.position;
        Dissolved?.Invoke(this);
    }
}