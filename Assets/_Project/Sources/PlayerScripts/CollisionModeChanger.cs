using Dissolvable;
using UnityEngine;

namespace PlayerScripts
{
    [RequireComponent(typeof(BoxCollider))]
    public class CollisionModeChanger : MonoBehaviour
    {
        private DissolvableObject _dissolvable;

        private void OnTriggerEnter(Collider other)
        {
            _dissolvable = other.GetComponentInParent<DissolvableObject>();

            if (_dissolvable != null)
                _dissolvable.SetSpeculative();
        }

        private void OnTriggerExit(Collider other)
        {
            _dissolvable = other.GetComponentInParent<DissolvableObject>();

            if (_dissolvable != null)
                _dissolvable.SetDynamic();
        }
    }
}