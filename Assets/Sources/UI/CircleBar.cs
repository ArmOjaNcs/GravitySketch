using Assets.Sources.Utils;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Assets.Sources.UI
{
    public abstract class CircleBar : PauseableAnimation
    {
        [SerializeField] private protected SmoothedImage ActiveTimeImage;
        [SerializeField] private protected SmoothedImage ReloadTimeImage;
        [SerializeField] private protected TextMeshProUGUI Text;

        private protected virtual void OnEnable()
        {
            ActiveTimeImage.Updated += OnActiveImageUpdated;
            ReloadTimeImage.Updated += OnReloadImageUpdated;
        }

        private protected override void OnDisable()
        {
            ActiveTimeImage.Updated -= OnActiveImageUpdated;
            ReloadTimeImage.Updated -= OnReloadImageUpdated;
            base.OnDisable();
        }

        private void Start()
        {
            ActiveTimeImage.gameObject.SetActive(false);
            ReloadTimeImage.gameObject.SetActive(false);
        }

        private protected override Tween GetAnimation()
        {
            return AnimationSpawner.GetShakeAnimation(Text.rectTransform, 0.5f);
        }

        private protected abstract void OnActiveImageUpdated();
        private protected abstract void OnReloadImageUpdated();
    }
}