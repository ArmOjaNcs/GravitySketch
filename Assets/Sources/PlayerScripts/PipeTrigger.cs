using Assets.Sources.Utils;
using UnityEngine;

namespace Assets.Sources.PlayerScripts
{
    public class PipeTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            other.gameObject.layer = UserUtils.FallingLayer;
            Debug.Log($"Triggered with {other.gameObject.name}");
        }
    }
}