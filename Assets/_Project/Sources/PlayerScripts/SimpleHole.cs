using UnityEngine;

public class SimpleHole : MonoBehaviour
{
    [SerializeField] private float _radius;
    [SerializeField] private Material _material;

    private void Awake()
    {
        _material.SetFloat("_HoleRadius", _radius);
        _material.SetVector("_HolePosition", new Vector4(
            transform.position.x, transform.position.y, transform.position.z, 0));
    }
}