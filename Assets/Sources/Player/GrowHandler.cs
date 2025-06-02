using System;
using UnityEngine;

public class GrowHandler : MonoBehaviour
{
    [SerializeField] private CubesCollector _collector;
    [SerializeField] private int _maxSize;
    [SerializeField] private int _growDelta;

    private int _previousCubesCount;
    private int _currentSize;
    private int _cubesOnNextGrow;

    public event Action Dissolved;
    public event Action Growing;
    public event Action GrowingDown;

    public int CurrentSize => _currentSize;
    public int CubesOnNextGrow => _cubesOnNextGrow;
    public int PreviousCubesCount => _previousCubesCount;
    public int GrowDelta => _growDelta;
    public bool IsCanGrowUp => _currentSize < _maxSize;

    private void OnEnable()
    {
        _collector.CubesCountChanged += OnCubesCountChanged;
    }

    private void OnDisable()
    {
        _collector.CubesCountChanged -= OnCubesCountChanged;
    }

    private void Awake()
    {
        _cubesOnNextGrow = _growDelta;
    }

    private void OnCubesCountChanged(int cubesCount)
    {
        if (cubesCount < _previousCubesCount)
            GrowDown();

        if (_currentSize < _maxSize && cubesCount >= _cubesOnNextGrow)
        {
            _cubesOnNextGrow += _growDelta /*+ _currentSize*//* * 2*/;
            GrowUp();
        }
        //Debug.Log($"Cubes count {cubesCount} on next grow" + _cubesOnNextGrow + $"Current size {_currentSize}");
        _previousCubesCount = cubesCount;
    }

    private void GrowUp()
    {
        _currentSize++;
        Growing?.Invoke();
    }

    private void GrowDown()
    {
        if (_currentSize == 0)
        {
            Dissolved?.Invoke();
            return;
        }

        _currentSize--;
        GrowingDown?.Invoke();
    }
}