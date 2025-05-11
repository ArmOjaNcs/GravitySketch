using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Score _score;

    private void Start()
    {
        _text.text = _score.Value.ToString();
    }

    private void OnEnable()
    {
        _score.ValueChanged += OnValueChanged;
    }

    private void OnDisable()
    {
        _score.ValueChanged -= OnValueChanged;
    }

    private void OnValueChanged()
    {
        _text.text = _score.Value.ToString();
    }
}