using Assets.Sources.Dissolvable;
using Assets.Sources.Utils;
using UnityEngine;

namespace Assets.Sources.PlayerScripts
{
    public class SimpleCatcher : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == UserUtils.NormalLayer)
            {
                Physics.SyncTransforms();
                other.gameObject.layer = UserUtils.FallingLayer;

                DissolvableObject dissolvableObject = other.GetComponentInParent<DissolvableObject>();

                if (dissolvableObject != null)
                    dissolvableObject.SetSpeculative();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == UserUtils.FallingLayer)
            {
                Physics.SyncTransforms();
                other.gameObject.layer = UserUtils.NormalLayer;

                DissolvableObject dissolvableObject = other.GetComponentInParent<DissolvableObject>();

                if (dissolvableObject != null)
                    dissolvableObject.SetDynamic();
            }
        }
    }
}