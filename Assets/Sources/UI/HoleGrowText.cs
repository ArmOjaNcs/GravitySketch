using Assets.Sources.Audio;
using Assets.Sources.Pause;
using Assets.Sources.PlayerScripts;
using Assets.Sources.Utils;
using TMPro;
using UnityEngine;

namespace Assets.Sources.UI
{
    public class HoleGrowText : PauseableRoutine
    {
        private const string GrowUp = "Grow UP";

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
            _growText.text = GrowUp;
            _growText.gameObject.SetActive(false);
            _audioPlayer.Init(pauseHandler);
            _audioPlayer.AudioSource.playOnAwake = false;
            _audioPlayer.AudioSource.loop = false;
            _smoothedFade.Init(pauseHandler);
            _smoothedFade.SetStartAplpha(UserUtils.HalfUnit);
            _smoothedFade.FadeOut();
            IsInitialized = true;
        }

        private protected override void OnRoutineStart()
        {
            _smoothedFade.ShowElements();
            _audioPlayer.Play();
        }

        private protected override void OnRoutineEnd()
        {
            _smoothedFade.FadeOut();
            _audioPlayer.Stop();
        }
    }
}