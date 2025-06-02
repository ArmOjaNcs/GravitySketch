using TMPro;
using UnityEngine;

public class UIPlayerCubesOnNextGrow : MonoBehaviour
{
    private const string CubesOnNextSize = "Cubes on next size: ";

    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private GrowHandler _growHandler;

    private void OnEnable()
    {
        _growHandler.Growing += OnGrowing;
        _growHandler.GrowingDown += OnGrowing;
    }

    private void OnDisable()
    {
        _growHandler.Growing -= OnGrowing;
        _growHandler.GrowingDown -= OnGrowing;
    }

    private void Start()
    {
        OnGrowing();
    }

    private void OnGrowing()
    {
        _text.text = CubesOnNextSize + _growHandler.CubesOnNextGrow;
    }
}