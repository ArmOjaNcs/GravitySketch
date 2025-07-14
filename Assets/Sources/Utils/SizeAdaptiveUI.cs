using Assets.Sources.PlayerScripts;
using UnityEngine;

namespace Assets.Sources.Utils
{
    public class SizeAdaptiveUI : MonoBehaviour
    {
        [SerializeField] private Transform _parent;
        [SerializeField] private GrowHandler _growHandler;
        [SerializeField, Min(0)] private float _growDelta;
        [SerializeField] private Vector3 _offset = new Vector3(2.5f, 0, 0);

        private Transform _cameraTransform;
        private Vector3 _growDeltaSize;

        private void OnEnable()
        {
            _growHandler.Growing += OnGrowing;
        }

        private void OnDisable()
        {
            _growHandler.Growing -= OnGrowing;
        }

        void Start()
        {
            _cameraTransform = Camera.main.transform;
            transform.SetParent(null);
            _growDeltaSize = transform.localScale / 6;

            if (_offset.x < 0)
                _growDelta *= -1;
        }

        void LateUpdate()
        {
            transform.position = _parent.position + _offset;
            transform.forward = _cameraTransform.forward;
        }

        private void OnGrowing()
        {
            _offset.x += _growDelta;
            transform.localScale += _growDeltaSize;
        }
    }
}