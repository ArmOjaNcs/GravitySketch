using Assets.Sources.Pause;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Sources.UI
{
    public class Scroller : PauseableObject
    {
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private float _scrollSpeed = 0.2f;
        [SerializeField] private ScrollInputDetector _detector;
        [SerializeField] private CanvasGroup _scrollViewGroup = null;

        private void OnEnable()
        {
            Subscribe();
        }

        private void OnDisable()
        {
            UnSubscribe();
        }

        public override void Pause()
        {
            base.Pause();
            SwitchScrollViewInteractable(false);
            UnSubscribe();
        }

        public override void Resume()
        {
            base.Resume();
            SwitchScrollViewInteractable(true);
            Subscribe();
        }

        private void Subscribe()
        {
            _detector.BeginDrag += OnBeginDrag;
            _detector.Dragging += OnDragging;
            _detector.EndDrag += OnEndDrag;
            _detector.ScrolledByWheel += OnScrolledByMouseWheel;
        }

        private void UnSubscribe()
        {
            _detector.BeginDrag -= OnBeginDrag;
            _detector.Dragging -= OnDragging;
            _detector.EndDrag -= OnEndDrag;
            _detector.ScrolledByWheel -= OnScrolledByMouseWheel;
        }

        private void OnBeginDrag(PointerEventData eventData) => _scrollRect.OnBeginDrag(eventData);
        private void OnDragging(PointerEventData eventData) => _scrollRect.OnDrag(eventData);
        private void OnEndDrag(PointerEventData eventData) => _scrollRect.OnEndDrag(eventData);

        private void OnScrolledByMouseWheel(float deltaWheel)
        {
            if (Mathf.Abs(deltaWheel) < 0.001f)
                return;

            float newPosition = _scrollRect.verticalNormalizedPosition + deltaWheel * _scrollSpeed;
            _scrollRect.verticalNormalizedPosition = Mathf.Clamp01(newPosition);
        }

        private void SwitchScrollViewInteractable(bool status)
        {
            if (_scrollViewGroup == null)
                return;

            _scrollViewGroup.interactable = status;
            _scrollViewGroup.blocksRaycasts = status;
        }
    }
}