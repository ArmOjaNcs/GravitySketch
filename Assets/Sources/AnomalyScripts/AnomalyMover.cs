using UnityEngine;
using Assets.Sources.Pause;
using Assets.Sources.PlayerScripts;

namespace Assets.Sources.AnomalyScpipts
{
    [RequireComponent(typeof(Anomaly))]
    public class AnomalyMover : PointMover
    {
        private Anomaly _anomaly;
        private bool _isMove;

        private void OnDisable()
        {
            if(_anomaly != null)
                _anomaly.IsDowned -= OnDowned;
        }

        private protected override void Update()
        {
            if (_isMove == false)
                return;

            base.Update();
        }

        private protected override void FixedUpdate()
        {
            if (_isMove == false)
                return;

            base.FixedUpdate();
        }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            
            _anomaly = GetComponent<Anomaly>();
            _anomaly.IsDowned += OnDowned;
            _isMove = true;
        }

        private void OnDowned() => _isMove = false;
    }
}