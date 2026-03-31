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
        [SerializeField] private Transform _forward;

        [Header("World bounds (half size)")]
        [SerializeField] private float _worldHalfWidth;
        [SerializeField] private float _worldHalfHeight;

        [Header("Icon size")]
        [SerializeField] private float _baseIconSize = 15f;
        [SerializeField] private float _targetBaseScale = 1f;
        [SerializeField] private float _targetBaseRadius = 0.65f;

        private RectTransform _mapRect;
        private bool _isInitialized;

        private void Awake()
        {
            _mapRect = _map.GetComponent<RectTransform>();
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
                worldPos.x / _worldHalfWidth,
                worldPos.y / _worldHalfHeight
            );

            float mapWidth = _mapRect.rect.width;
            float mapHeight = _mapRect.rect.height;

            float mapAspect = mapWidth / mapHeight;
            float worldAspect = _worldHalfWidth / _worldHalfHeight;

            Vector2 miniPos;

            if (mapAspect > worldAspect)
            {
                float scale = mapHeight * 0.5f;

                miniPos = new Vector2(
                    normalized.x * scale * worldAspect,
                    normalized.y * scale
                );
            }
            else
            {
                float scale = mapWidth * 0.5f;

                miniPos = new Vector2(
                    normalized.x * scale,
                    normalized.y * scale / worldAspect
                );
            }

            _icon.anchoredPosition = miniPos;
        }

        private void UpdateRotation()
        {
            float angle = -_forward.eulerAngles.y;
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

            _icon.localScale = Vector3.one * scaleFactor;
        }

        public void SetMapSprite(Sprite sprite)
        {
            _map.sprite = sprite;
            _isInitialized = true;
        }
    }
}