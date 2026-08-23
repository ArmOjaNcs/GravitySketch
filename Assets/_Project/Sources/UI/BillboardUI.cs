using UnityEngine;

namespace UI
{
    public class BillboardUI : MonoBehaviour
    {
        [SerializeField] private protected Transform Parent;
        [SerializeField] private protected Vector3 Offset;

        private protected Transform CameraPivot;
        private Transform _cameraTransform;
        private Transform _transform;
        private bool _isStop;

        private protected Quaternion PivotRotation => Quaternion.Euler(
            0, CameraPivot.eulerAngles.y, 0);

        private void Start()
        {
            _transform = transform;
            _cameraTransform = Camera.main.transform;
            CameraPivot = _cameraTransform.parent;
            _transform.SetParent(null);
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
            _transform.position = Parent.position + Offset;
            _transform.rotation = PivotRotation;
        }
    }
}