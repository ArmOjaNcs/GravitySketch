using System.Collections;
using TMPro;
using UnityEngine;

public class HealthSmoothedText : HealthSmoothedView
{
    [SerializeField] private TextMeshProUGUI _text;

    private void Start()
    {
        _text.text = Health.CurrentValue + "/" + Health.MaxValue;
        SmoothDuration = CurrentSmoothDuration;
    }

    private protected override IEnumerator UpdateView()
    {
        float elapsedTime = 0;
        string[] splitedText = _text.text.Split('/');
        float startValue = float.Parse(splitedText[0]);

        while (elapsedTime < SmoothDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedPosition = elapsedTime / SmoothDuration;
            float currentValue = Mathf.Lerp(startValue, Health.CurrentValue, normalizedPosition);
            _text.text = (Mathf.Round(currentValue)).ToString() + "/" + Health.MaxValue;

            yield return null;
        }

        _text.text = Health.CurrentValue + "/" + Health.MaxValue;
    }
}