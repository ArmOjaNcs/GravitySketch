using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;

    private void LateUpdate()
    {
        transform.position = _mainCamera.transform.position;
        transform.rotation = _mainCamera.transform.rotation;
    }
}