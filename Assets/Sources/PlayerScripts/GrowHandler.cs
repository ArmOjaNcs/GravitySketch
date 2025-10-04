using System;
using UnityEngine;

namespace Assets.Sources.PlayerScripts
{
    public class GrowHandler : MonoBehaviour
    {
        [SerializeField] private CubesCollector _collector;
        [SerializeField] private int _maxSize;
        [SerializeField] private int _growDelta;

        private int _currentSize;
        private int _cubesOnNextGrow;

        public event Action Growing;

        public int CurrentSize => _currentSize;
        public int CubesOnNextGrow => _cubesOnNextGrow;
        public int GrowDelta => _growDelta;
        public bool IsCanGrow => _currentSize < _maxSize;

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
            if (_currentSize < _maxSize && cubesCount >= _cubesOnNextGrow)
            {
                _cubesOnNextGrow += _growDelta;
                GrowUp();
            }
        }

        private void GrowUp()
        {
            _currentSize++;
            Growing?.Invoke();
        }
    }
}