using UnityEngine;

[RequireComponent (typeof(BoxCollider))]
public class SpawnZone : MonoBehaviour
{
    private Transform _transform;
    private BoxCollider _boxCollider;
   
    public Vector3 GetPoint(Vector3 randomPoint)
    {
        if(_transform == null)
            _transform = transform;

        return _transform.TransformPoint(randomPoint);
    }

    public BoxCollider GetBoxCollider()
    {
        if (_boxCollider == null)
            return _boxCollider = GetComponent<BoxCollider>();

        return _boxCollider;
    }
}