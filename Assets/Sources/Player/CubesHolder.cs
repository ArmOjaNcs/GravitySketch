using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[Serializable]
public class CubesHolder
{
    private Queue<IReadonlyTemplateCube> _cubesQueue = new Queue<IReadonlyTemplateCube>();
    private List<IReadonlyTemplateCube> _toThrowBack = new List<IReadonlyTemplateCube>();

    public IReadOnlyList<IReadonlyTemplateCube> Cubes => _cubesQueue.ToArray();

    public void AddCube(IReadonlyTemplateCube templateCube) => _cubesQueue.Enqueue(templateCube);

    public IEnumerable<IReadonlyTemplateCube> GetCubes(float percent)
    {
        if (_cubesQueue.Count == 0)
        {
            Debug.Log($"cubes count: {_cubesQueue.Count}");
            return Enumerable.Empty<IReadonlyTemplateCube>();
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