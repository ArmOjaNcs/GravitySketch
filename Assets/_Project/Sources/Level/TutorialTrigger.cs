using Assets.Sources.Utils;
using System;
using UnityEngine;

namespace Assets.Sources.Level
{
    [RequireComponent(typeof(Collider))]
    public class TutorialTrigger : MonoBehaviour
    {
        [SerializeField] private TutorialType _type;

        private Collider _collider;

        public event Action<TutorialType> PlayerInZone;

        public TutorialType Type => _type;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _collider.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(UserUtils.Player))
                PlayerInZone?.Invoke(_type);
        }

        public void EnableCollider()
        {
            if(_collider != null )
                _collider.enabled = true;
        }
    }
}