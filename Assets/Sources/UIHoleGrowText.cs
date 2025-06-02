using System.Collections;
using TMPro;
using UnityEngine;

public class UIHoleGrowText : MonoBehaviour
{
    private const string GrowUp = "Grow UP";
    private const string GrowDown = "Grow Down";

    [SerializeField] private TextMeshProUGUI _growText;
    [SerializeField] private GrowHandler _growHandler;

    private Coroutine _coroutine;

    private void OnEnable()
    {
        _growHandler.Growing += OnGrowing;
        _growHandler.GrowingDown += OnGrowingDown;
    }

    private void OnDisable()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _growHandler.Growing -= OnGrowing;
        _growHandler.GrowingDown -= OnGrowingDown;
    }

    private void Start()
    {
        _growText.enabled = false;
    }

    private IEnumerator ShowGrowStatus(string status)
    {
        _growText.enabled = true;
        _growText.text = status;

        yield return new WaitForSeconds(1);

        _growText.text = "";
        _growText.enabled = false;
    }

    private void OnGrowing()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(ShowGrowStatus(GrowUp));
    }

    private void OnGrowingDown()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(ShowGrowStatus(GrowDown));
    }
}