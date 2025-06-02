using System.Collections;
using UnityEngine;

public class HoleMaskHandler : MonoBehaviour
{
    [SerializeField] private Mover _mover;
    [SerializeField] private Material _material;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Grower _grower;
    [SerializeField] private float _growDuration;

    private Transform _transform;
    private float _targetRadius;
    private float _currentRadius;

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

    private void Awake()
    {
        _targetRadius = _mover.transform.lossyScale.x / 2;
        _material.SetFloat("_HoleRadius", _targetRadius);
        _currentRadius = _targetRadius;
        _transform = transform;
        _renderer.material = _material;
    }

    private void OnPositionChanged(Vector3 position)
    {
        _material.SetVector("_HolePosition", new Vector4(position.x, _transform.position.y, position.z, 0));
    }

    private void OnSizeChanged(float sizeDelta)
    {
       // Debug.Log("Radius " + _targetRadius);
        _targetRadius += sizeDelta / 2;
        StartCoroutine(SizeChangeRoutine());
    }

    private IEnumerator SizeChangeRoutine()
    {
        float elapsedTime = 0;

        while (elapsedTime < _growDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedPosition = elapsedTime / _growDuration;
            _currentRadius = Mathf.Lerp(_currentRadius, _targetRadius, normalizedPosition);
            _material.SetFloat("_HoleRadius", _currentRadius);
            yield return null;
        }

        _currentRadius = _targetRadius;
    }
}