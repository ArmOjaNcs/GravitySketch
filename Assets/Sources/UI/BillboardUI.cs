using UnityEngine;

namespace Assets.Sources.UI
{
    public class BillboardUI : MonoBehaviour
    {
        [SerializeField] private protected Transform Parent;
        [SerializeField] private protected Vector3 Offset = new Vector3(0, 2f, 0);

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

            FollowByParrent();
        }

        public void IsStop(bool isStop) => _isStop = isStop;

        private protected virtual void FollowByParrent()
        {
            transform.position = Parent.position + Offset;
            transform.forward = _cameraTransform.forward;
        }
    }
}