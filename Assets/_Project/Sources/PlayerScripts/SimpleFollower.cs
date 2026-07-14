using UnityEngine;

namespace Assets.Sources.PlayerScripts
{
    public class SimpleFollower : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _yOffset;

        private Transform _transform;
        private Vector3 _position = Vector3.zero;
        private Vector3 _baseLocalScale;

        private Vector3 PositionWithOffset => new Vector3(_position.x, _yOffset, _position.z);

        private Vector3 TargetScale => _target.lossyScale;

        private void Awake()
        {
            _transform = transform;
            _baseLocalScale = _transform.localScale;
            _transform.SetParent(null);
        }

        private void Update()
        {
            _position = _target.position;
            _transform.position = PositionWithOffset;

            _transform.localScale = new Vector3(
               _baseLocalScale.x * TargetScale.x,
               _baseLocalScale.y * TargetScale.y,
               _baseLocalScale.z * TargetScale.z);
        }
    }
}