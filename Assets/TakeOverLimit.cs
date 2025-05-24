using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))] 
public class TakeOverLimit : MonoBehaviour
{
    public event Action<SimpleCube> CubeAbsorbed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out SimpleCube simpleCube))
            CubeAbsorbed?.Invoke(simpleCube);
    }
}