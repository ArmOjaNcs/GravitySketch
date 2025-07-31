using Assets.Sources.Pause;
using DG.Tweening;

namespace Assets.Sources.UI
{
    public abstract class PauseableAnimation : PauseableObject
    {
        private protected Tween Animation;
        private protected bool WasPlayingBeforePause;

        private protected override void Awake()
        {
            base.Awake();
            Animation = GetAnimation();
        }

        private protected virtual void OnDisable()
        {
            if (Animation.IsActive())
                Animation.Kill();
        }

        public override void Pause()
        {
            base.Pause();

            if (Animation.IsActive() && Animation.IsPlaying())
            {
                Animation.Pause();
                WasPlayingBeforePause = true;
            }
        }

        public override void Resume()
        {
            base.Resume();

            if (Animation.IsActive() && WasPlayingBeforePause)
            {
                Animation.Play();
                WasPlayingBeforePause = false;
            }
        }

        private protected abstract Tween GetAnimation();
    }
}