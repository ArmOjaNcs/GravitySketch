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
    private Vector3 _targetOffset;

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

    private void Awake()
    {
        _cinemachineTransposer = _virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        _targetOffset = _cinemachineTransposer.m_FollowOffset;
    }

    private void OnGrowing()
    {
        _targetOffset += _offsetByGrow;
        StartCoroutine(GrowRoutine());
    }

    private void OnGrowingDown()
    {
        _targetOffset -= _offsetByGrow;
        StartCoroutine(GrowRoutine());
    }

    private IEnumerator GrowRoutine()
    {
        float elapsedTime = 0;
       
        while (elapsedTime < _growDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedPosition = elapsedTime / _growDuration;
            _cinemachineTransposer.m_FollowOffset = Vector3.Lerp(_cinemachineTransposer.m_FollowOffset, 
                _targetOffset, normalizedPosition);
      
            yield return null;
        }

        _cinemachineTransposer.m_FollowOffset = _targetOffset;
    }
}