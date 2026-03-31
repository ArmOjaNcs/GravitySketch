using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.UI
{
    public class ScrollRectSliderBinder : MonoBehaviour
    {
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private Slider slider;

        private void Awake()
        {
            slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        private void OnDestroy()
        {
            slider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }

        private void OnSliderValueChanged(float sliderValue)
        {
            scrollRect.verticalNormalizedPosition = 1f - sliderValue;
        }
    }
}