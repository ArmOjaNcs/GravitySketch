using Assets.Sources.Pause;
using DG.Tweening;

namespace Assets.Sources.UI
{
    public abstract class PauseableAnimation : PauseableObject
    {
        private protected Sequence Animation;
        private protected bool WasPlayingBeforePause;

        private protected virtual void OnDisable()
        {
            if (Animation.IsActive())
                Animation.Kill();
        }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            Animation = GetAnimation();
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

        public void Play()
        {
            if (Animation.IsActive())
                Animation.Restart();
        }

        private protected abstract Sequence GetAnimation();
    }
}