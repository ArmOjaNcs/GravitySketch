using System;
using UnityEngine;

public class EnemyMoveZone : MonoBehaviour
{
    private Transform _player;

    public event Action PlayerDetected;
    public event Action PlayerLosed;

    public Transform Player => _player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(_player == null)
                _player = other.transform;
            
            PlayerDetected?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            PlayerLosed?.Invoke();
    }
}