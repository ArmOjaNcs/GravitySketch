using UnityEngine;

namespace Assets.Sources.UI
{
    public class BillboardUI : MonoBehaviour
    {
        [SerializeField] private Transform _parent;
        [SerializeField] private Vector3 _offset = new Vector3(0, 2f, 0);

        private Transform _cameraTransform;
        private bool _isStop;

        private void Start()
        {
            _cameraTransform = Camera.main.transform;
            transform.SetParent(null);
        }

        private void LateUpdate()
        {
            if (_isStop)
                return;

            transform.position = _parent.position + _offset;
            transform.forward = _cameraTransform.forward;
        }

        public void IsStop(bool isStop) => _isStop = isStop;
    }
}