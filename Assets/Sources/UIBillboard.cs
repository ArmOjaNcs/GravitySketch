using UnityEngine;

public class UIBillboard : MonoBehaviour
{
    [SerializeField] private Transform _parent;
    [SerializeField] private Vector3 _offset = new Vector3(0, 2f, 0);

    private Transform _cameraTransform;

    void Start()
    {
        _cameraTransform = Camera.main.transform;
        transform.SetParent(null);
    }

    void LateUpdate()
    {
        transform.position = _parent.position + _offset;
        transform.forward = _cameraTransform.forward;
    }
}