using TMPro;
using UnityEngine;

public class UIHoleSizeText : MonoBehaviour
{
    private const string CurrentSize = "Current size: ";

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
        _text.text = CurrentSize + _growHandler.CurrentSize;
    }
}