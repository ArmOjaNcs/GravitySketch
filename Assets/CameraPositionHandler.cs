using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraPositionHandler : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private GrowHandler _growHandler;
    [SerializeField] private float _growDuration;
    [SerializeField] private Vector3 _offsetByGrow;

    private CinemachineTransposer _cinemachineTransposer;

    private void Awake()
    {
        _cinemachineTransposer = _virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
    }

    private void OnEnable()
    {
        _growHandler.Growing += OnGrowing;
        _growHandler.GrowingDown += OnGrowingDown;
    }

    private void OnDisable()
    {
        _growHandler.Growing -= OnGrowing;
        _growHandler.GrowingDown -= OnGrowingDown;
    }

    private void OnGrowing()
    {
        Vector3 targetOffset = _cinemachineTransposer.m_FollowOffset + _offsetByGrow;
        StartCoroutine(GrowRoutine(targetOffset));
    }

    private void OnGrowingDown()
    {
        Vector3 targetOffset = _cinemachineTransposer.m_FollowOffset - _offsetByGrow;
        StartCoroutine(GrowRoutine(targetOffset));
    }

    private IEnumerator GrowRoutine(Vector3 targetOffset)
    {
        float elapsedTime = 0;
       
        while (elapsedTime < _growDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedPosition = elapsedTime / _growDuration;
            _cinemachineTransposer.m_FollowOffset = Vector3.MoveTowards(_cinemachineTransposer.m_FollowOffset, 
                targetOffset, normalizedPosition);
      
            yield return null;
        }

        _cinemachineTransposer.m_FollowOffset = targetOffset;
    }
}