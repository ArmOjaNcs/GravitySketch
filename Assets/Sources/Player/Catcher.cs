using System;
using UnityEngine;

[RequireComponent (typeof(CapsuleCollider))]
public class Catcher : MonoBehaviour
{
    [SerializeField] private int _normalLayer; 
    [SerializeField] private int _fallingLayer;
    [SerializeField] private Transform _takeOverLimit;

    public event Action<SimpleCube> CubeCatched;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _normalLayer)
            other.gameObject.layer = _fallingLayer;

        if (other.TryGetComponent(out SimpleCube simpleCube))
            simpleCube.DropDown();
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.transform.position.y < _takeOverLimit.position.y)
        {
            if(other.TryGetComponent(out SimpleCube simpleCube))
                CubeCatched?.Invoke(simpleCube);

            return;
        }

        if (other.gameObject.layer == _fallingLayer)
            other.gameObject.layer = _normalLayer;
    }
}