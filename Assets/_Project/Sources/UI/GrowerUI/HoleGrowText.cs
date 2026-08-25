using Audio;
using Pause;
using PlayerScripts;
using TMPro;
using Utils;
using UI.PauseableRoutineUI;
using UnityEngine;

namespace UI.GrowerUI
{
    public class HoleGrowText : PauseableRoutine
    {
        [SerializeField] private TextMeshProUGUI _growText;
        [SerializeField] private GrowHandler _growHandler;
        [SerializeField] private SmoothedFade _smoothedFade;
        [SerializeField] private AudioPlayer _audioPlayer;

        private void OnEnable()
        {
            _growHandler.Growing += OnUpdate;
        }

        private protected override void OnDisable()
        {
            _growHandler.Growing -= OnUpdate;
            base.OnDisable();
        }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            _growText.gameObject.SetActive(false);
            _audioPlayer.Init(pauseHandler);
            _audioPlayer.AudioSource.playOnAwake = false;
            _audioPlayer.AudioSource.loop = false;
            _smoothedFade.Init(pauseHandler);
            _smoothedFade.FadeOut(0);
            IsInitialized = true;
        }

        private protected override void OnRoutineStart()
        {
            _smoothedFade.FadeIn(UserUtils.MinFadeDuration, UserUtils.MaxAlpha);
            _audioPlayer.Play();
        }

        private protected override void OnRoutineEnd()
        {
            _smoothedFade.FadeOut(UserUtils.FadeDuration);
            _audioPlayer.Stop();
        }
    }
}