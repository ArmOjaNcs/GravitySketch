using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CubesHolder
{
    private Queue<SimpleCube> _cubesQueue = new Queue<SimpleCube>();
    private List<SimpleCube> _toThrowBack = new List<SimpleCube>();

    public IReadOnlyList<SimpleCube> Cubes => _cubesQueue.ToArray();

    public void AddCube(SimpleCube simpleCube) => _cubesQueue.Enqueue(simpleCube);

    public int Count => _cubesQueue.Count;

    public IReadOnlyList<SimpleCube> GetCubes(int cubesCount)
    {
        if (_cubesQueue.Count == 0)
        {
            Debug.Log($"cubes count: {_cubesQueue.Count}");
            return null;
        }

        _toThrowBack.Clear();

        cubesCount = Mathf.Clamp(cubesCount, cubesCount, _cubesQueue.Count);

        for (int index = 0; index < cubesCount; index++)
            _toThrowBack.Add(_cubesQueue.Dequeue());

        return _toThrowBack;
    }
}