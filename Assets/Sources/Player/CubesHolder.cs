using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[Serializable]
public class CubesHolder
{
    private Queue<SimpleCube> _cubesQueue = new Queue<SimpleCube>();
    private List<SimpleCube> _toThrowBack = new List<SimpleCube>();

    public IReadOnlyList<SimpleCube> Cubes => _cubesQueue.ToArray();

    public void AddCube(SimpleCube simpleCube) => _cubesQueue.Enqueue(simpleCube);

    public int Count => _cubesQueue.Count;

    public IEnumerable<SimpleCube> GetCubes(float percent)
    {
        if (_cubesQueue.Count == 0)
        {
            Debug.Log($"cubes count: {_cubesQueue.Count}");
            return Enumerable.Empty<SimpleCube>();
        }

        if (percent <= 0)
            throw new ArgumentOutOfRangeException("percent can not be 0 or less");

        if (percent > 100)
            throw new ArgumentOutOfRangeException("percent can not be more than 100");

        percent /= 100;
        percent = Mathf.Clamp01(percent);

        _toThrowBack.Clear();
        int cubesCount = Mathf.Max(1, Mathf.RoundToInt(_cubesQueue.Count * percent));

        for (int index = 0; index < cubesCount; index++)
            _toThrowBack.Add(_cubesQueue.Dequeue());

        return _toThrowBack.ToArray();
    }
}