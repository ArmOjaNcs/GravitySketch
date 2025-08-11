using Assets.Sources.Pause;
using Assets.Sources.PlayerScripts;
using Assets.Sources.SimpleCubeScripts;
using Assets.Sources.Utils;
using UnityEngine;

namespace Assets.Sources.UI
{
    public class CubesCountUI : SmoothedText
    {
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

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            StartText = UserUtils.CubesCount;
            IsNeedToSplit = true;
            SplitSign = '/';
            MaxValue = _simpleCubeSpawner.TotalCubes;
            EndText = GetEndText();
            Text.text = GetTotalText();
            IsInitialized = true;
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