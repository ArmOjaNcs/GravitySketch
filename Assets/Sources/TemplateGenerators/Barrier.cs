using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Barrier : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private BoxCollider _collider;

    public MeshRenderer MeshRenderer => _meshRenderer;
    public BoxCollider Collider => _collider;
}