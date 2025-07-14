using System;
using System.Collections;
using UnityEngine;
using Assets.Sources.Utils;
using Assets.Sources.PlayerScripts;

namespace Assets.Sources.EnemyScripts
{
    [RequireComponent(typeof(SphereCollider))]
    public class EnemyMoveZone : MonoBehaviour
    {
        private Player _player;
        private SphereCollider _sphereCollider;
        private WaitForEndOfFrame _waitForEndOfFrame;

        public event Action PlayerDetected;
        public event Action PlayerLosed;

        public Player Player => _player;

        private void Awake()
        {
            _sphereCollider = GetComponent<SphereCollider>();
            _waitForEndOfFrame = new WaitForEndOfFrame();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(UserUtils.Player))
            {
                if (_player == null)
                    _player = other.GetComponent<Player>();

                PlayerDetected?.Invoke();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag(UserUtils.Player))
                PlayerLosed?.Invoke();
        }

        public void Refresh() => StartCoroutine(RefreshRoutine());

        private IEnumerator RefreshRoutine()
        {
            _sphereCollider.enabled = false;
            yield return _waitForEndOfFrame;
            _sphereCollider.enabled = true;
        }
    }
}