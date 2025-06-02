using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))] 
public class TakeOverLimit : MonoBehaviour
{
    [SerializeField] private Transform _hole;

    public event Action<SimpleCube> CubeAbsorbed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out SimpleCube simpleCube))
            CubeAbsorbed?.Invoke(simpleCube);

        if (other.TryGetComponent(out Enemy enemy))
            enemy.Dissolve(_hole);
    }
}