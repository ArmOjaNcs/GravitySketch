using Assets.Sources.ColorizerScripts;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.UI
{
    public class ColorizerView : MonoBehaviour
    {
        [SerializeField] private Image[] _colors;
        [SerializeField] private Image _arrow;
        [SerializeField] private Colorizer _colorizer;

        private void OnEnable()
        {
            _colorizer.QueueChanged += OnQueueChanged;
        }

        private void OnDisable()
        {
            _colorizer.QueueChanged -= OnQueueChanged;
        }

        private void Start()
        {
            Vector2 startAnchoredPos = _arrow.rectTransform.anchoredPosition;
            Vector3 startScale = _arrow.rectTransform.localScale;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(_arrow.rectTransform.DOAnchorPosX(startAnchoredPos.x - 50f, 0.75f).SetEase(Ease.Linear));
            sequence.Insert(0, _arrow.rectTransform.DOScale(startScale * 0.75f, 0.75f).SetEase(Ease.Linear));
            sequence.SetLoops(-1, LoopType.Yoyo);
            sequence.SetLink(gameObject);
            sequence.Restart();
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
    }
}