using Assets.Sources.PlayerScripts;
using Assets.Sources.SimpleCubeScripts;
using Assets.Sources.Utils;
using TMPro;
using UnityEngine;

namespace Assets.Sources.UI
{
    public class CubesCountUI : MonoBehaviour
    {
        [SerializeField] private CubesCollector _cubesCollector;
        [SerializeField] private SimpleCubeSpawner _simpleCubeSpawner;
        [SerializeField] private TextMeshProUGUI _valueText;
        [SerializeField] private TextMeshProUGUI _maxText;
        [SerializeField] private SmoothedSlider _slider;

        private int _maxValue;

        private void OnEnable()
        {
            _cubesCollector.CubesCountChanged += OnCubesCountChanged;
            _simpleCubeSpawner.CubesSpawned += OnCubesSpawned;
        }

        private void OnDisable()
        {
            _cubesCollector.CubesCountChanged -= OnCubesCountChanged;
            _simpleCubeSpawner.CubesSpawned -= OnCubesSpawned;
        }

        private void Start()
        {
            _slider.SetStartValue(0);
            _valueText.text = _cubesCollector.CubesCount.ToString();
        }

        private void OnCubesCountChanged(int count)
        {
            _valueText.text = _cubesCollector.CubesCount.ToString();
            float target = _maxValue > 0 ? (float)_cubesCollector.CubesCount / _maxValue : 0f;
            _slider.UpdateValue(UserUtils.Unit, target);
        }

        private void OnCubesSpawned()
        {
            _maxValue = _simpleCubeSpawner.TotalCubes;
            _maxText.text = _maxValue.ToString();
        }
    }
}