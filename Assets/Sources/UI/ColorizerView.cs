using Assets.Sources.ColorizerScripts;
using Assets.Sources.Pause;
using Assets.Sources.Utils;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.UI
{
    public class ColorizerView : PauseableAnimation
    {
        [SerializeField] private Image[] _colors;
        [SerializeField] private Image _arrow;
        [SerializeField] private Colorizer _colorizer;

        private void OnEnable()
        {
            _colorizer.QueueChanged += OnQueueChanged;
        }

        private protected override void OnDisable()
        {
            _colorizer.QueueChanged -= OnQueueChanged;
            base.OnDisable();
        }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            Animation.Restart();
        }

        private void OnQueueChanged(IEnumerable<Color> colors)
        {
            Debug.Log("ColorsCount" + colors.Count());
            if (colors.Count() >= _colors.Length)
            {
                for (int i = 0; i < _colors.Length; i++)
                    _colors[i].color = colors.ElementAt(i);
            }
            else
            {
                for (int i = 0; i < colors.Count(); i++)
                {
                    if (_colors[i].isActiveAndEnabled)
                        _colors[i].color = colors.ElementAt(i);
                }

                _colors[colors.Count()].gameObject.SetActive(false);

                if (_colors[0].isActiveAndEnabled == false)
                    _arrow.gameObject.SetActive(false);
            }
        }

        private protected override Sequence GetAnimation()
        {
            return AnimationSpawner.GetArrowAnimation(_arrow.rectTransform);
        }
    }
}