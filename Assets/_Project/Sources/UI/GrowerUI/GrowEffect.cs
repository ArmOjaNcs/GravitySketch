using Pause;
using PlayerScripts;
using Utils;
using UnityEngine;

namespace UI.GrowerUI
{
    public class GrowEffect : PauseableRoutine
    {
        [SerializeField] private GrowHandler _growHandler;
        [SerializeField] private ParticleSystem _effect;
        [SerializeField] private BillboardUI _billboard;
        [SerializeField] private Transform _parent;

        private Transform _transform;
        private bool _isPlayed;

        private void OnEnable()
        {
            _growHandler.Growing += OnGrowing;
        }

        private protected override void OnDisable()
        {
            _growHandler.Growing -= OnGrowing;
            base.OnDisable();
        }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            _transform = transform;
            _effect.Stop();
            _billboard.IsStop(true);
            IsInitialized = true;
        }

        public override void Pause()
        {
            base.Pause();
            _effect.Pause();
        }

        public override void Resume()
        {
            base.Resume();

            if (_isPlayed)
                _effect.Play();
        }

        private protected override void OnRoutineStart()
        {
            _effect.gameObject.SetActive(true);
            _billboard.IsStop(false);
            _effect.Play();
            _isPlayed = true;
            _transform.localScale = _parent.lossyScale;
        }

        private protected override void OnRoutineEnd()
        {
            base.OnRoutineEnd();
            _effect.Stop();
            _effect.gameObject.SetActive(false);
            _isPlayed = false;
            _billboard.IsStop(true);
        }

        private void OnGrowing()
        {
            UpdateView(UserUtils.Two);
        }
    }
}