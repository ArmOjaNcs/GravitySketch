using System;
using System.Collections;
using UnityEngine;

public class Grower : MonoBehaviour
{
    [SerializeField] private GrowHandler _growHandler;
    [SerializeField] private ParticleSystem[] _particleSystems;
    [SerializeField] private Vector3 _sizeDelta;
    [SerializeField] private float _growSize;
    [SerializeField] private float _growDuration;
    [SerializeField] private Catcher _catcher;

    private Transform _player;
    private Vector3 _targetScale;

    public event Action<float> SizeChanged;
    int count = 0;

    private void Awake()
    {
        _player = transform;
        _targetScale = _player.lossyScale;
    }

    private void OnEnable()
    {
        _growHandler.Growing += OnGrowing;
        _growHandler.GrowingDown += OnGrowingDown;
    }

    private void OnDisable()
    {
        _growHandler.Growing -= OnGrowing;
        _growHandler.GrowingDown -= OnGrowingDown;
    }

    private void OnGrowingDown()
    {
        CalculateTargetScale(true);
        StartCoroutine(GrowRoutine());
        SizeChanged?.Invoke(-_growSize);
    }

    private void OnGrowing()
    {
        CalculateTargetScale(false);
        StartCoroutine(GrowRoutine());
        SizeChanged?.Invoke(_growSize);
        _catcher.RefreshSensor();
        count++;
        //Debug.Log("Growing level " + count);
    }

    private void CalculateTargetScale(bool isNegative)
    {
        int sign = 1;

        if (isNegative)
            sign = -1;
        
        //Debug.Log("currentScale" + _player.lossyScale);
        _targetScale += _sizeDelta * sign;
        //Debug.Log("targetScale" + _targetScale);
    }

    private IEnumerator GrowRoutine()
    {
        float elapsedTime = 0;

        while (elapsedTime < _growDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedPosition = elapsedTime / _growDuration;
            _player.localScale = Vector3.Lerp(_player.localScale, _targetScale, normalizedPosition);

            foreach (ParticleSystem particle in _particleSystems)
            {
                particle.transform.localScale = Vector3.Lerp(particle.transform.localScale,
                    _targetScale, normalizedPosition);
            }

            yield return null;
        }

        _player.localScale = _targetScale;

        foreach (var particle in _particleSystems)
            particle.transform.localScale = _targetScale;

        //Debug.Log($"local: {_player.localScale}, lossy: {_player.lossyScale}");
    }
}