using DG.Tweening;
using System;
using UnityEngine;

public class Grower : MonoBehaviour
{
    [SerializeField] private GrowHandler _growHandler;
    [SerializeField] private Transform _player;
    [SerializeField] private float _growSize;

    public event Action<float> SizeChanged;

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
        Vector3 holeScale = _player.localScale;
        holeScale.x -= _growSize;
        holeScale.z -= _growSize;
        _player.DOScale(holeScale, 1).Play();
        SizeChanged?.Invoke(-_growSize);
    }

    private void OnGrowing()
    {
        Vector3 holeScale = _player.localScale;
        holeScale.x += _growSize;
        holeScale.z += _growSize;
        _player.DOScale(holeScale, 1).Play();
        SizeChanged?.Invoke(_growSize);
        Debug.Log("Growing");
    }
}