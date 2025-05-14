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

        Vector3 startPoint = _transform.TransformPoint(randomPoint);
        startPoint.z = _transform.position.z;
        return startPoint;
    }

    public BoxCollider GetBoxCollider()
    {
        if (_boxCollider == null)
            return _boxCollider = GetComponent<BoxCollider>();

        return _boxCollider;
    }
}