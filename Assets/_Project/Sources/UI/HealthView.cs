using Assets.Sources.Pause;
using Assets.Sources.Utils;
using UnityEngine;

namespace Assets.Sources.UI
{
    public class HealthView : PauseableObject
    {
        [SerializeField] private Health _health;
        [SerializeField] private SmoothedImage _fillerNear;
        [SerializeField] private SmoothedImage _fillerFar;
        [SerializeField] private float _duration;

        private float _targetValue;
        private bool _isNearUpdated;
        private bool _isFarUpdated;
        private float _previousValue;

        private void OnEnable()
        {
            _health.Updated += OnUpdate;
            _fillerFar.Updated += OnFarFillerUpdate;
            _fillerNear.Updated += OnNearFillerUpdate;
        }

        private void OnDisable()
        {
            _health.Updated -= OnUpdate;
            _fillerFar.Updated -= OnFarFillerUpdate;
            _fillerNear.Updated -= OnNearFillerUpdate;
        }

        private void Start()
        {
            _fillerFar.SetValue(1);
            _fillerNear.SetValue(1);
            _previousValue = _health.MaxValue;
        }

        public override void Init(PauseHandler pauseHandler)
        {
            _fillerNear.Init(pauseHandler);
            _fillerFar.Init(pauseHandler);
            IsInitialized = true;
        }

        private void OnUpdate()
        {
            float difference = _health.CurrentValue - _previousValue;
            _previousValue = _health.CurrentValue;
            _targetValue = _health.CurrentValue / _health.MaxValue;
            _isFarUpdated = false;
            _isNearUpdated = false;

            if(difference >= 0)
                _fillerFar.UpdateValue(_duration, _targetValue);
            else
                _fillerNear.UpdateValue(_duration, _targetValue);
        }

        private void OnFarFillerUpdate()
        {
            _isFarUpdated = true;

            if(TryStop())
                return;

            _fillerNear.UpdateValue(_duration, _targetValue);
        }

        private void OnNearFillerUpdate()
        {
            _isNearUpdated = true;

            if(TryStop()) 
                return;

            _fillerFar.UpdateValue(_duration, _targetValue);
        }

        private bool TryStop()
        {
            if(_isFarUpdated && _isNearUpdated)
            {
                _fillerNear.SetValue(_targetValue);
                _fillerFar.SetValue(_targetValue);
                return true;
            }

            return false;
        }
    }
}