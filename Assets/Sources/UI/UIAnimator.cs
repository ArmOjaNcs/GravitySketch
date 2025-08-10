using DG.Tweening;
using UnityEngine;

namespace Assets.Sources.UI
{
    public abstract class UIAnimator : MonoBehaviour
    {
        [SerializeField] private protected RectTransform RectTransform;
        [SerializeField] private protected float Duration;

        private protected CanvasGroup CanvasGroup;
        private protected Sequence ShowAnimation;
        private protected Sequence HideAnimation;

        public bool IsShown { get; protected set; }

        private void Awake()
        {
            CanvasGroup = RectTransform.GetComponent<CanvasGroup>();
            InitAnimations();
        }

        private void OnDestroy()
        {
            ShowAnimation?.Kill();
            HideAnimation?.Kill();
        }

        public virtual void Show()
        {
            IsShown = true;
            HideAnimation?.Pause();
            gameObject.SetActive(true);
            ShowAnimation?.Restart();
        }

        public virtual void Hide()
        {
            IsShown = false;
            ShowAnimation?.Pause();
            CanvasGroup.interactable = false;
            HideAnimation?.Restart();
        }

        private protected abstract void InitAnimations();
    }
}