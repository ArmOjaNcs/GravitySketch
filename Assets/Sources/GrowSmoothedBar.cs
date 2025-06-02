using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GrowSmoothedBar : MonoBehaviour
{
    [SerializeField] private Image _ringImage;
    [SerializeField] private GrowHandler _growHandler;
    [SerializeField] private CubesCollector _cubesCollector;
    [SerializeField] private float _smoothDuration;
    [SerializeField] private float _showDuration;
    [SerializeField] private TextMeshProUGUI _text;

    private Coroutine _coroutine;
    private WaitForSeconds _showWait;
    private bool _isGrowing;
    private float _sliderValueInGrowDelta;
    private float _sliderValueOutGrowDelta;

    private void Awake()
    {
        _showWait = new WaitForSeconds(_showDuration);
        _isGrowing = true;
    }

    private void OnEnable()
    {
        _cubesCollector.CubesCountChanged += OnCubesUpdate;
        _growHandler.Growing += OnGrowing;
        _growHandler.GrowingDown += OnGrowingDown;
    }

    private void OnDisable()
    {
        _cubesCollector.CubesCountChanged -= OnCubesUpdate;
        _growHandler.Growing -= OnGrowing;
        _growHandler.GrowingDown -= OnGrowingDown;

        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }

    private void Start()
    {
        _ringImage.fillAmount = 0;
        _ringImage.gameObject.SetActive(false);
        _text.enabled = false;
    }

    private void OnGrowing() => _isGrowing = true;
    private void OnGrowingDown() => _isGrowing = false;

    private void OnCubesUpdate(int cubesCount)
    {
        if (_growHandler.IsCanGrowUp == false)
            return;

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(UpdateView(cubesCount));
    }

    private IEnumerator UpdateView(int cubesCount)
    {
        _ringImage.gameObject.SetActive(true);

        float startValue = 0;
        float elapsedTime = 0;
        float targetValue = 0;
        int previousGrowThreshold = _growHandler.CubesOnNextGrow - _growHandler.GrowDelta;
        int currentCubes = 0;

        if (_isGrowing)
        {
            startValue = _sliderValueInGrowDelta;
            targetValue = ((float)cubesCount - previousGrowThreshold) / _growHandler.GrowDelta;
            currentCubes = cubesCount - previousGrowThreshold;
            _text.text = currentCubes + "/" + _growHandler.GrowDelta;
        }
        else
        {
            startValue = _sliderValueOutGrowDelta;
            targetValue = (float)cubesCount / _growHandler.CubesOnNextGrow;
            _text.text = cubesCount + "/" + _growHandler.CubesOnNextGrow;
        }

        _text.enabled = true;

        _sliderValueInGrowDelta = ((float)cubesCount - previousGrowThreshold) / _growHandler.GrowDelta;
        _sliderValueOutGrowDelta = (float)cubesCount / _growHandler.CubesOnNextGrow;

        while (elapsedTime < _smoothDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedPosition = elapsedTime / _smoothDuration;
            _ringImage.fillAmount = Mathf.Lerp(startValue, targetValue, normalizedPosition);

            yield return null;
        }

        _ringImage.fillAmount = targetValue;

        yield return _showWait;
        _ringImage.gameObject.SetActive(false);
        _text.enabled = false;
    }
}