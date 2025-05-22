using UnityEngine;

[RequireComponent (typeof(BoxCollider))]
public class Pusher : MonoBehaviour
{
    [SerializeField] private float _pushForce;
    
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Mover mover))
            mover.ApplyExternalForce((mover.transform.position - _transform.position).normalized 
                * _pushForce * Time.deltaTime);

        if (other.TryGetComponent(out SimpleCube simpleCube))
            simpleCube.DropDown();
    }
}