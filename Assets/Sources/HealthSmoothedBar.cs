using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthSmoothedBar :HealthSmoothedView
{
    [SerializeField] private Slider _slider;

    private void Start()
    {
        SmoothDuration = CurrentSmoothDuration;
        _slider.value = Health.CurrentValue / Health.MaxValue;
    }

    private protected override IEnumerator UpdateView()
    {
        float elapsedTime = 0;
        float startValue = _slider.value;
        float targetValue = Health.CurrentValue / Health.MaxValue;

        while (elapsedTime < SmoothDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedPosition = elapsedTime / SmoothDuration;
            _slider.value = Mathf.Lerp(startValue, targetValue, normalizedPosition);

            yield return null;
        }

        _slider.value = targetValue;
    }
}