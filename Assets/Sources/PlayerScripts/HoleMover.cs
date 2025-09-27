using UnityEngine;

namespace Assets.Sources.PlayerScripts
{
    public class HoleMover : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private float _fixedY = 0f;
        [SerializeField] private float _speed = 10f;
        [SerializeField] private float _radius;

        private Material _material;
        private Transform _transform;

        public void Init(Material material)
        {
            _transform = transform;
            _material = material;
            _material.SetFloat("_HoleRadius", _radius);
            _material.SetVector("_HolePosition", new Vector4(_transform.position.x,
                    _transform.position.y, _transform.position.z, 0));
        }

        private void Update()
        {
            FollowByMouse();
        }

        private void FollowByMouse()
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = _camera.ScreenPointToRay(mousePosition);
            Plane plane = new Plane(Vector3.up, new Vector3(0, _fixedY, 0));

            if (plane.Raycast(ray, out float distance))
            {
                Vector3 targetPoint = ray.GetPoint(distance);
                Vector3 newPosition = Vector3.MoveTowards(_transform.position, targetPoint, _speed * Time.deltaTime);
                newPosition.y = _fixedY;
                _transform.position = newPosition;
                _material.SetVector("_HolePosition", new Vector4(_transform.position.x, 
                    _transform.position.y, _transform.position.z, 0));
            }
        }
    }
}