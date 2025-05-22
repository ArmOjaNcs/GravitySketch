using System.Collections;
using UnityEngine;

public class HoleMaskHandler : MonoBehaviour
{
    [SerializeField] private Transform _hole;
    [SerializeField] private Mover _mover;
    [SerializeField] private Material _material;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Grower _grower;
    [SerializeField] private float _growDuration;

    private Transform _transform;
    private float _radius;
    private float _currentRadius;

    private void Awake()
    {
        _radius = _hole.localScale.x / 2;
        _material.SetFloat("_HoleRadius", _radius);
        _currentRadius = _radius;
        Debug.Log(_hole.localScale.x/2);
        _transform = transform;
        _renderer.material = _material;
    }

    private void OnEnable()
    {
        _mover.PositionChanged += OnPositionChanged;
        _grower.SizeChanged += OnSizeChanged;
    }

    private void OnDisable()
    {
        _mover.PositionChanged -= OnPositionChanged;
        _grower.SizeChanged -= OnSizeChanged;
    }

    private void OnPositionChanged(Vector3 position)
    {
        _material.SetVector("_HolePosition", new Vector4(position.x, _transform.position.y, position.z, 0));
    }

    private void OnSizeChanged(float sizeDelta)
    {
        Debug.Log("Radius " + _radius);
        _radius += sizeDelta / 2;
        StartCoroutine(SizeChangeRoutine());
    }

    private IEnumerator SizeChangeRoutine()
    {
        float elapsedTime = 0;

        while (elapsedTime < _growDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedPosition = elapsedTime / _growDuration;
            _currentRadius = Mathf.MoveTowards(_currentRadius, _radius, normalizedPosition);
            _material.SetFloat("_HoleRadius", _currentRadius);
            yield return null;
        }

        _currentRadius = _radius;
    }
}