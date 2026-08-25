using ColorizerScripts;
using TMPro;
using UI.PauseableRoutineUI;
using Utils;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ColorizerUI
{
    public class ColorCubeView : MonoBehaviour
    {
        [SerializeField] private ColorData _colorData;
        [SerializeField] private Image _cube;
        [SerializeField] private Image _cubeBackground;
        [SerializeField] private SmoothedFade _frameFade;
        [SerializeField] private TextMeshProUGUI _count;

        private void OnEnable()
        {
            Subscribe();
        }

        private void OnDisable()
        {
            UnSubscribe();
        }

        private void Subscribe()
        {
            _colorData.CountChanged += OnCountChanged;
            _colorData.Selected += OnColorDataSelected;
            _colorData.Deselected += OnColorDataDeselected;
            _colorData.Initialized += OnColorDataInitialized;
        }

        private void UnSubscribe()
        {
            _colorData.CountChanged -= OnCountChanged;
            _colorData.Selected -= OnColorDataSelected;
            _colorData.Deselected -= OnColorDataDeselected;
            _colorData.Initialized -= OnColorDataInitialized;
        }

        private void OnColorDataInitialized()
        {
            _cube.color = _colorData.Color;
            _cubeBackground.color = UserUtils.GetOppositeColor(_colorData.Color);
            _count.text = _colorData.Count.ToString();
        }

        private void OnCountChanged()
        {
            _count.text = _colorData.Count.ToString();
        }

        private void OnColorDataSelected(ColorData _) => _frameFade.FadeIn(UserUtils.FadeDuration, UserUtils.MaxAlpha);

        private void OnColorDataDeselected() => _frameFade.FadeOut(UserUtils.FadeDuration);
    }
}