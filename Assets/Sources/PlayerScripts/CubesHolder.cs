using Assets.Sources.SimpleCubeScripts;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.PlayerScripts
{
    [Serializable]
    public class CubesHolder
    {
        private Queue<SimpleCube> _cubesQueue = new Queue<SimpleCube>();

        public IReadOnlyList<SimpleCube> Cubes => _cubesQueue.ToArray();

        public void AddCube(SimpleCube simpleCube) => _cubesQueue.Enqueue(simpleCube);

        public int Count => _cubesQueue.Count;

        public void RemoveCubes(int cubesCount)
        {
            if (_cubesQueue.Count == 0)
            {
                Debug.Log($"cubes count: {_cubesQueue.Count}");
                return;
            }

            cubesCount = Mathf.Clamp(cubesCount, 0, _cubesQueue.Count);

            for (int index = 0; index < cubesCount; index++)
                _cubesQueue.Dequeue();
        }
    }
}