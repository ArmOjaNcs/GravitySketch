using UnityEngine;

[RequireComponent (typeof(CapsuleCollider))]
public class Catcher : MonoBehaviour
{
    [SerializeField] private int _normalLayer; 
    [SerializeField] private int _fallingLayer;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _normalLayer)
            other.gameObject.layer = _fallingLayer;

        if (other.TryGetComponent(out SimpleCube simpleCube))
            simpleCube.DropDown();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == _fallingLayer)
            other.gameObject.layer = _normalLayer;
    }
}