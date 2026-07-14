using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Sources.Utils
{
    public class Focus : MonoBehaviour
    {
        [SerializeField] private EventSystem _eventSystem;
        [SerializeField] private bool _isRestoreOnStart;

        private void Start()
        {
            if (_isRestoreOnStart)
                RestoreFocus();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus)
                RestoreFocus();
        }

        private void OnApplicationPause(bool pause)
        {
            if (!pause)
                RestoreFocus();
        }

        private void RestoreFocus()
        {
            StartCoroutine(RestoreWhenReady());
        }

        private IEnumerator RestoreWhenReady()
        {
            yield return null;
            _eventSystem.enabled = false;
            yield return null;
            _eventSystem.enabled = true;
        }
    }
}