using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Sources.SimpleCubeScripts;
using UnityEngine;

namespace Assets.Sources.PlayerScripts
{
    public class CubesCollector : MonoBehaviour
    {
        [SerializeField] private TakeOverLimit _takeOverLimit;
        [SerializeField] private Transform _hole;

        private CubesHolder _holder = new CubesHolder();

        public event Action<int> CubesCountChanged;

        public int CubesCount => _holder.Count;

        private void OnEnable()
        {
            _takeOverLimit.CubeAbsorbed += OnCubeAbsorbed;
        }

        private void OnDisable()
        {
            _takeOverLimit.CubeAbsorbed -= OnCubeAbsorbed;
        }

        public void InvokeCubesCountChanged()
        {
            CubesCountChanged?.Invoke(_holder.Count);
        }

        public List<Color> GetAllCollors()
        {
            List<Color> colors = new List<Color>();

            foreach (SimpleCube simpleCube in _holder.Cubes)
                colors.Add(simpleCube.Color);

            return colors;
        }

        private void OnCubeAbsorbed(SimpleCube simpleCube)
        {
            if (_holder.Cubes.Contains(simpleCube))
                return;

            _holder.AddCube(simpleCube);
            CubesCountChanged?.Invoke(_holder.Count);
            simpleCube.Dissolve(_hole);
        }
    }
}