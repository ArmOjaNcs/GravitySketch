using PlayerScripts;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MapIcon : MonoBehaviour
    {
        [Header("Mini-map UI")]
        [SerializeField] private Image _map;
        [SerializeField] private RectTransform _icon;
        private float _mapWidth;
        private float _mapHeight;

        [Header("Target")]
        [SerializeField] private Transform _target;
        [SerializeField] private Transform _forward;
        [SerializeField] private Grower _grower;

        [Header("World bounds (half size)")]
        [SerializeField] private float _worldHalfWidth;
        [SerializeField] private float _worldHalfHeight;

        [Header("Icon size")]
        [SerializeField] private float _baseIconSize = 15f;
        [SerializeField] private float _targetBaseScale = 1f;

        private RectTransform _mapRect;
        private bool _isInitialized;
        private bool _isGrowing;

        private void Awake()
        {
            RectInit();
            InitDimensions();
            _icon.sizeDelta = Vector2.one * _baseIconSize;
        }

        private void OnEnable()
        {
            _grower.StartGrow += OnStartGrow;
            _grower.Updated += OnGrowerUpdated;
        }

        private void OnDisable()
        {
            _grower.StartGrow -= OnStartGrow;
            _grower.Updated -= OnGrowerUpdated;
        }

        private void Update()
        {
            if (_isInitialized == false)
                return;

            UpdatePosition();
            UpdateRotation();
            UpdateScale();
        }

        private void OnRectTransformDimensionsChange()
        {
            RectInit();
            InitDimensions();
        }

        public void SetMapSprite(Sprite sprite)
        {
            _map.sprite = sprite;
            _isInitialized = true;
        }

        private void RectInit()
        {
            if (_mapRect != null)
                return;

            _mapRect = _map.GetComponent<RectTransform>();
        }

        private void InitDimensions()
        {
            if (_mapRect == null)
                return;

            var rect = _mapRect.rect;
            _mapWidth = rect.width;
            _mapHeight = rect.height;
        }

        private void UpdatePosition()
        {
            Vector2 worldPos = new Vector2(_target.position.x, _target.position.z);

            Vector2 normalized = new Vector2(
                worldPos.x / _worldHalfWidth,
                worldPos.y / _worldHalfHeight);

            float mapWidth = _mapWidth;
            float mapHeight = _mapHeight;

            float mapAspect = mapWidth / mapHeight;
            float worldAspect = _worldHalfWidth / _worldHalfHeight;

            Vector2 miniPos;

            if (mapAspect > worldAspect)
            {
                float scale = mapHeight * 0.5f;

                miniPos = new Vector2(
                    normalized.x * scale * worldAspect,
                    normalized.y * scale);
            }
            else
            {
                float scale = mapWidth * 0.5f;

                miniPos = new Vector2(
                    normalized.x * scale,
                    normalized.y * scale / worldAspect);
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
            if (_isGrowing == false)
                return;

            float playerScale = _target.localScale.x;

            float scaleFactor = playerScale <= _targetBaseScale
                ? 1f
                : (playerScale / _targetBaseScale);

            _icon.localScale = Vector3.one * scaleFactor;
        }

        private void OnGrowerUpdated() => _isGrowing = false;

        private void OnStartGrow() => _isGrowing = true;
    }
}