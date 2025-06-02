using System;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class EnemyStopZone : MonoBehaviour
{
    public event Action<bool> IsStopped; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            IsStopped?.Invoke(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            IsStopped?.Invoke(false);
    }
}