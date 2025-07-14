using UnityEngine;
using Assets.Sources.Utils;
using Assets.Sources.PlayerScripts;
using Assets.Sources.SimpleCubeScripts;

namespace Assets.Sources.UI
{
    public class CubesCountUI : SmoothedText
    {
        private const string CubesCount = "Cubes collected: ";

        [SerializeField] private CubesCollector _cubesCollector;
        [SerializeField] private SimpleCubeSpawner _simpleCubeSpawner;

        private void OnEnable()
        {
            _cubesCollector.CubesCountChanged += OnCubesCountChanged;
        }

        private protected override void OnDisable()
        {
            base.OnDisable();
            _cubesCollector.CubesCountChanged -= OnCubesCountChanged;
        }

        private void Start()
        {
            StartText = CubesCount;
            IsNeedToSplit = true;
            SplitSign = '/';
            MaxValue = _simpleCubeSpawner.TotalCubes;
            EndText = GetEndText();
            Text.text = GetTotalText();
        }

        private void OnCubesCountChanged(int count)
        {
            TargetValue = _cubesCollector.CubesCount;
            EndText = GetEndText();
            OnUpdate();
        }

        private float GetPercent()
        {
            return MaxValue > 0 ? _cubesCollector.CubesCount / MaxValue : 0f;
        }

        private string GetEndText()
        {
            string endText = string.Empty;
            float percent = GetPercent();
            percent = Mathf.Clamp01(percent);
            Text.color = UserUtils.GetColorByPercentage(percent);
            return endText = " " + (percent * 100).ToString("F2") + "%";
        }
    }
}