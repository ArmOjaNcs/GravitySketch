using TMPro;
using UnityEngine;

public class UICubesCountText : MonoBehaviour
{
    private const string CubesCount = "Cubes count: ";

    [SerializeField] private CubesCollector _cubesCollector;
    [SerializeField] private TextMeshProUGUI _text;

    private void OnEnable()
    {
        _cubesCollector.CubesCountChanged += OnCubesCountChanged;
    }

    private void OnDisable()
    {
        _cubesCollector.CubesCountChanged -= OnCubesCountChanged;
    }

    private void OnCubesCountChanged(int count)
    {
        _text.text = CubesCount + count;
    }
}