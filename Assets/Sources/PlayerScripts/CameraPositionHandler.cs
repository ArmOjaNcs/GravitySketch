using Assets.Sources.Pause;
using Cinemachine;
using UnityEngine;

namespace Assets.Sources.PlayerScripts
{
    public class CameraPositionHandler : PauseableRoutine
    {
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private GrowHandler _growHandler;
        [SerializeField] private Vector3 _offsetByGrow;

        private CinemachineTransposer _cinemachineTransposer;
        private Vector3 _targetOffset;

        private void OnEnable()
        {
            _growHandler.Growing += OnGrowing;
        }

        private protected override void OnDisable()
        {
            base.OnDisable();
            _growHandler.Growing -= OnGrowing;
        }

        private protected override void Awake()
        {
            base.Awake();
            _cinemachineTransposer = _virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
            _targetOffset = _cinemachineTransposer.m_FollowOffset;
        }

        private void OnGrowing()
        {
            _targetOffset += _offsetByGrow;
            OnUpdate();
        }

        private protected override void OnRoutineIteration(float cycleDuration) 
        {
            float progress = ElapsedTime / cycleDuration;
            _cinemachineTransposer.m_FollowOffset = Vector3.Lerp(_cinemachineTransposer.m_FollowOffset,
                _targetOffset, progress);
        }

        private protected override void OnRoutineEnd()
        {
            _cinemachineTransposer.m_FollowOffset = _targetOffset;
            base.OnRoutineEnd();
        }
    }
}