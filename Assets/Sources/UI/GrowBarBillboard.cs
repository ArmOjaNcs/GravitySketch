using Assets.Sources.PlayerScripts;
using Assets.Sources.Utils;
using UnityEngine;

namespace Assets.Sources.UI
{
    public class GrowBarBillboard : BillboardUI
    {
        [SerializeField] private Grower _grower;

        private RectTransform _rectTransform;
        private Vector3 _defaultScale;
        private Vector3 _currentScale;
        private float _defaultRotationX;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _defaultScale = _rectTransform.localScale;
            _currentScale = UserUtils.GetCorrectScale(_defaultScale, Parent.lossyScale);
            _defaultRotationX = _rectTransform.eulerAngles.x;
        }

        private void OnEnable()
        {
            _grower.ScaleChanged += OnScaleChanged;
        }

        private void OnDisable()
        {
            _grower.ScaleChanged -= OnScaleChanged;
        }

        private void OnScaleChanged()
        {
            _currentScale = UserUtils.GetCorrectScale(_defaultScale, Parent.lossyScale);
        }

        private protected override void FollowByParrent()
        {
            float targetY = Parent.eulerAngles.y;
            _rectTransform.rotation = Quaternion.Euler(_defaultRotationX, 0f, -targetY);
            _rectTransform.localScale = _currentScale;
            _rectTransform.position = Parent.position + Offset;
        }
    }
}