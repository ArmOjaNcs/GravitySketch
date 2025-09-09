using Assets.Sources.Pause;
using Assets.Sources.Utils;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class Aim : PauseableAnimation
    {
        [SerializeField] private Image _image;

        private RectTransform _rectTransform;
        private RectTransform _imageRectTransform;

        public override void Init(PauseHandler pauseHandler)
        {
            _rectTransform = GetComponent<RectTransform>();
            _imageRectTransform = _image.GetComponent<RectTransform>();
            base.Init(pauseHandler);
        }

        public void SetColor(Color color) => _image.color = color;
        public void SetPosition(Vector3 position) => _rectTransform.position = position;
        public void StartAnimaton() => Animation.Play();

        private protected override Sequence GetAnimation()
        {
            return AnimationSpawner.GetAimAnimation(_imageRectTransform);
        }
    }
}