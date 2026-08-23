using UnityEngine;

namespace EnemyScripts
{
    public class FansAnimator : MonoBehaviour
    {
        [SerializeField] private Transform[] _fans;
        [SerializeField] private float _speed = 2000f;
        [SerializeField] private bool _isActive;

        private void Update()
        {
            if (_isActive == false)
                return;

            foreach (Transform fan in _fans)
                fan.Rotate(Vector3.forward * _speed * Time.deltaTime);
        }

        public void Activate() => _isActive = true;

        public void Deactivate() => _isActive = false;
    }
}