using UnityEngine;

namespace Assets.Sources.PlayerScripts
{
    public class HoleMover : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private float _fixedY = 0f;
        [SerializeField] private float _speed = 10f;
        [SerializeField] private float _radius;
        [SerializeField] private RectTransform _cursorUI;

        private Material _material;
        private Transform _transform;
        private bool _isStarted;
        private bool _isUseJoystick;

        private void Update()
        {
            if (_isStarted == false)
                return;

            if (_isUseJoystick)
                FollowByJoystick();
            else
                FollowByMouse();
        }

        public void Init(Material material)
        {
            _transform = transform;
            _material = material;
            _material.SetFloat("_HoleRadius", _radius);
            _material.SetVector("_HolePosition", new Vector4(
                    _transform.position.x,
                    _transform.position.y,
                    _transform.position.z,
                    0));
        }

        public void SetStarted() => _isStarted = true;

        public void EnableJoystickControl(bool value) => _isUseJoystick = value;

        private void FollowByMouse()
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = _camera.ScreenPointToRay(mousePosition);
            Plane plane = new Plane(Vector3.up, new Vector3(0, _fixedY, 0));

            if (plane.Raycast(ray, out float distance))
            {
                Vector3 targetPoint = ray.GetPoint(distance);
                MoveHole(targetPoint);
            }
        }

        private void FollowByJoystick()
        {
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, _cursorUI.position);

            Ray ray = _camera.ScreenPointToRay(screenPos);
            Plane plane = new Plane(Vector3.up, new Vector3(0, _fixedY, 0));

            if (plane.Raycast(ray, out float distance))
            {
                Vector3 worldPos = ray.GetPoint(distance);
                MoveHole(worldPos);
            }
        }

        private void MoveHole(Vector3 target)
        {
            Vector3 newPosition = Vector3.MoveTowards(_transform.position, target, _speed * Time.deltaTime);
            newPosition.y = _fixedY;
            _transform.position = newPosition;

            UpdateShaderPosition();
        }

        private void UpdateShaderPosition()
        {
            _material.SetVector(
                "_HolePosition",
                new Vector4(_transform.position.x, _transform.position.y, _transform.position.z, 0));
        }
    }
}