using System;
using System.Collections;
using UnityEngine;
using Assets.Sources.Utils;

namespace Assets.Sources.EnemyScripts
{
    [RequireComponent(typeof(SphereCollider))]
    public class EnemyStopZone : MonoBehaviour
    {
        private SphereCollider _sphereCollider;
        private WaitForEndOfFrame _waitForEndOfFrame;

        public event Action<bool> ShouldStop;

        private void Awake()
        {
            _sphereCollider = GetComponent<SphereCollider>();
            _waitForEndOfFrame = new WaitForEndOfFrame();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(UserUtils.Player))
                ShouldStop?.Invoke(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag(UserUtils.Player))
                ShouldStop?.Invoke(false);
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