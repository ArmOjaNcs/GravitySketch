using System;
using UnityEngine;

public class GrowHandler : MonoBehaviour
{
    [SerializeField] private CubesCollector _collector;
    [SerializeField] private int _maxSize;

    private int _cubesOnNextGrow = 10;
    private int _damageCount;
    private int _previousCubesCount;
    private int _currentSize;

    public event Action Dissolved;
    public event Action Growing;
    public event Action GrowingDown;

    private void OnEnable()
    {
        _collector.CubesCountChanged += OnCubesCountChanged;
    }

    private void OnDisable()
    {
        _collector.CubesCountChanged -= OnCubesCountChanged;
    }

    private void OnCubesCountChanged(int cubesCount)
    {
        if(cubesCount < _previousCubesCount)
        {
            _damageCount++;

            if(_damageCount >= 2)
            {
                _damageCount = 0;
                GrowDown();
            }
        }
        
        if (cubesCount == _cubesOnNextGrow)
        {
            _cubesOnNextGrow += _cubesOnNextGrow;
            GrowUp();
        }
       
        _previousCubesCount = cubesCount;
    }

    private void GrowUp()
    {
        if (_currentSize >= _maxSize)
            return;
        
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