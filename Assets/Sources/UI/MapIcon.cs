using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.UI
{
    public class MapIcon : MonoBehaviour
    {
        [Header("Mini-map UI")]
        [SerializeField] private Image _map;
        [SerializeField] private RectTransform _icon;

        [Header("Target")]
        [SerializeField] private Transform _target;

        [Header("World bounds (half size)")]
        [SerializeField] private float _worldHalfWidth;
        [SerializeField] private float _worldHalfHeight;

        [Header("Icon size")]
        [SerializeField] private float _baseIconSize = 15f;
        [SerializeField] private float _targetBaseScale = 1f;
        [SerializeField] private float _targetBaseRadius = 0.65f;

        private RectTransform _mapRect;
        private Vector2 _iconOriginalSize;
        private bool _isInitialized;

        private void Awake()
        {
            _mapRect = _map.GetComponent<RectTransform>();
            _iconOriginalSize = _icon.sizeDelta;
            _icon.sizeDelta = Vector2.one * _baseIconSize;
        }

        private void Update()
        {
            if (_isInitialized == false)
                return;

            UpdatePosition();
            UpdateRotation();
            UpdateScale();
        }

        private void UpdatePosition()
        {
            Vector2 worldPos = new Vector2(_target.position.x, _target.position.z);

            Vector2 normalized = new Vector2(
                Mathf.Clamp(worldPos.x / _worldHalfWidth, -1f, 1f),
                Mathf.Clamp(worldPos.y / _worldHalfHeight, -1f, 1f)
            );

            Vector2 miniPos = new Vector2(
                normalized.x * (_mapRect.rect.width * 0.5f),
                normalized.y * (_mapRect.rect.height * 0.5f)
            );

            _icon.anchoredPosition = miniPos;
        }

        private void UpdateRotation()
        {
            float angle = -_target.eulerAngles.y;
            _icon.localRotation = Quaternion.Euler(0, 0, angle);
        }

        private void UpdateScale()
        {
            float playerScale = Mathf.Max(
               _target.localScale.x,
               _target.localScale.y,
               _target.localScale.z
           );

            float scaleFactor = playerScale <= _targetBaseScale
                ? 1f
                : (playerScale / _targetBaseScale);

            float finalSize = _baseIconSize * scaleFactor;

            _icon.sizeDelta = new Vector2(finalSize, finalSize);
        }

        public void SetMapSprite(Sprite sprite)
        {
            _map.sprite = sprite;
            _isInitialized = true;
        }
    }
}