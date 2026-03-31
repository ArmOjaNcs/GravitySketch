using Assets.Sources.ColorizerScripts;
using Assets.Sources.Pause;
using Assets.Sources.Utils;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.UI
{
    public class ColorizerView : PauseableAnimation
    {
        [SerializeField] private Image _arrow;
        [SerializeField] private Colorizer _colorizer;
        [SerializeField] private Image _cube;
        [SerializeField] private Image _cubeBackground;
        [SerializeField] private SmoothedFade _fade;
        [SerializeField] private TextMeshProUGUI _count;

        private void OnEnable()
        {
            _colorizer.ColorsCountChanged += OnColorsCountChanged;
        }

        private protected override void OnDisable()
        {
            _colorizer.ColorsCountChanged -= OnColorsCountChanged;
            base.OnDisable();
        }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            _fade.Init(pauseHandler);
            Animation.Restart();
        }

        private void OnColorsCountChanged(Color color, int count)
        {
            if (color == Color.clear)
                _fade.FadeOut(UserUtils.HalfOfUnit);
            else
                _fade.FadeIn(UserUtils.HalfOfUnit, UserUtils.Unit);

            _cube.color = color;
            _cubeBackground.color = UserUtils.GetOppositeColor(color);
            _count.text = count.ToString();
        }

        private protected override Sequence GetAnimation()
        {
            return AnimationSpawner.GetMoveScaleAnimation(_arrow.rectTransform, new Vector2(-50, 0));
        }
    }
}