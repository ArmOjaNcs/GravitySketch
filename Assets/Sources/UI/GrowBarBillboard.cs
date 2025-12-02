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

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _defaultScale = _rectTransform.localScale;
            _currentScale = UserUtils.GetCorrectScale(_defaultScale, Parent.lossyScale);
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
            _rectTransform.localScale = _currentScale;
            base.FollowByParrent();
        }
    }
}