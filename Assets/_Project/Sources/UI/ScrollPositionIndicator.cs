using Assets.Sources.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.UI
{
    public class ScrollPositionIndicator : MonoBehaviour
    {
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private SmoothedFade _fade;
        [SerializeField] private ScrollInputDetector _inputDetector;
        [SerializeField] private RectTransform _handle;
        [SerializeField] private RectTransform _indicatorArea;
        [SerializeField] private VerticalScrollContent _verticalScrollContent;
        [SerializeField, Min(1)] private float _showTime;

        private Coroutine _routine;
        private float _timer;
        private bool _isInteracted;

        private void OnEnable()
        {
            _inputDetector.Interacted += OnInteracted;
            _inputDetector.InteractStopped += OnInteractStopped;
            _verticalScrollContent.Rebuilded += OnScrollContentRebuilded;
        }

        private void OnDisable()
        {
            _inputDetector.Interacted -= OnInteracted;
            _inputDetector.InteractStopped -= OnInteractStopped;
            _verticalScrollContent.Rebuilded -= OnScrollContentRebuilded;
            StopRoutine();
        }

        private void SetHandleHeight()
        {
            float viewHeight = _scrollRect.viewport.rect.height;
            Debug.Log($"viewHeight = {viewHeight}");
            float contentHeight = _scrollRect.content.rect.height;
            Debug.Log($"contentHeight = {contentHeight}");

            float ratio = Mathf.Clamp01(viewHeight / contentHeight);
            _handle.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _indicatorArea.rect.height * ratio);
        }

        private void OnInteracted()
        {
            _isInteracted = true;

            if (_routine == null && isActiveAndEnabled)
                _routine = StartCoroutine(ShowRoutine());
        }

        private void OnInteractStopped()
        {
            _isInteracted = false;
        }

        private void OnScrollContentRebuilded() => SetHandleHeight();

        private void UpdateHandlePosition()
        {
            float normalized = _scrollRect.verticalNormalizedPosition;
            float maxOffset = _indicatorArea.rect.height - _handle.rect.height;
            _handle.anchoredPosition = new Vector2(_handle.anchoredPosition.x, -maxOffset * (1f - normalized));
        }

        private IEnumerator ShowRoutine()
        {
            _fade.FadeIn(UserUtils.HalfOfUnit, UserUtils.HalfOfUnit);

            while (_timer < _showTime)
            {
                _timer += Time.deltaTime;
                UpdateHandlePosition();

                if (_isInteracted)
                    _timer = 0;

                yield return null;
            }

            _fade.FadeOut(UserUtils.HalfOfUnit);
            _timer = 0;
            _routine = null;
        }

        private void StopRoutine()
        {
            if (_routine != null)
            {
                StopCoroutine(_routine);
                _timer = 0;
                _routine = null;
            }
        }
    }
}