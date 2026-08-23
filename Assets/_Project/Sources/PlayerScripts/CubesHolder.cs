using System;
using System.Collections.Generic;
using SimpleCubeScripts;
using UnityEngine;

namespace PlayerScripts
{
    [Serializable]
    public class CubesHolder
    {
        private Queue<SimpleCube> _cubesQueue = new Queue<SimpleCube>();

        public IReadOnlyList<SimpleCube> Cubes => _cubesQueue.ToArray();

        public int Count => _cubesQueue.Count;

        public void AddCube(SimpleCube simpleCube) => _cubesQueue.Enqueue(simpleCube);
    }
}