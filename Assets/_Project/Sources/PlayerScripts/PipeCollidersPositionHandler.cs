using UnityEngine;

namespace Assets.Sources.PlayerScripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class PipeCollidersPositionHandler : MonoBehaviour
    {
        [SerializeField] private Transform _player;

        private Transform _transform;
        private Vector3 _baseLocalScale;
        private Rigidbody _rigidbody;

        private Vector3 PlayerScale => _player.lossyScale;

        private void Awake()
        {
            _transform = transform;
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            _rigidbody.constraints &= ~RigidbodyConstraints.FreezePositionX;
            _rigidbody.constraints &= ~RigidbodyConstraints.FreezePositionZ;
            _rigidbody.interpolation = RigidbodyInterpolation.None;
            _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            _rigidbody.isKinematic = true;
            _rigidbody.useGravity = false;
            _baseLocalScale = _transform.localScale;
            _transform.SetParent(null);
        }

        private void Update()
        {
            _transform.localScale = new Vector3(
               _baseLocalScale.x * PlayerScale.x,
               _baseLocalScale.y * PlayerScale.y,
               _baseLocalScale.z * PlayerScale.z);
        }

        private void FixedUpdate()
        {
            _rigidbody.MovePosition(_player.position);
        }
    }
}