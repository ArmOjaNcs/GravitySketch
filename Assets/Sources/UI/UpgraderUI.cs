using Assets.Sources.PlayerScripts;
using Assets.Sources.Utils;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Assets.Sources.UI
{
    public class UpgraderUI : PauseableAnimation
    {
        [SerializeField] private Upgrader _upgrader;
        [SerializeField] private SmoothedFade _smoothedFade;
        [SerializeField] private TextMeshProUGUI _moveSpeed;
        [SerializeField] private TextMeshProUGUI _boostSpeed;
        [SerializeField] private TextMeshProUGUI _defenceTime;
        [SerializeField] private TextMeshProUGUI _damage;
        [SerializeField] private TextMeshProUGUI _upgraded;
        [SerializeField] private RectTransform _textPivot;

        private void OnEnable()
        {
            _upgrader.Upgraded += OnUpgraded;
        }

        private protected override void OnDisable()
        {
            _upgrader.Upgraded -= OnUpgraded;
            base.OnDisable();   
        }

        private void Start()
        {
            _upgraded.text = UserUtils.Upgraded;
            _upgraded.gameObject.SetActive(false);
            UpdateUI();
        }

        private void OnUpgraded()
        {
            _smoothedFade.ShowElements();
            Animation.Restart();
            Animation.OnComplete(()=> _smoothedFade.FadeOut());
            UpdateUI();
        }

        private void UpdateUI()
        {
            _moveSpeed.text = UserUtils.MoveSpeed + _upgrader.MoveSpeed.ToString("F2");
            _boostSpeed.text = UserUtils.BoostSpeed + _upgrader.BoostSpeed.ToString("F2");
            _defenceTime.text = UserUtils.DefenceTime + _upgrader.DefendTime.ToString("F2");
            _damage.text = UserUtils.Damage + _upgrader.Damage.ToString("F2");
        }

        private protected override Sequence GetAnimation()
        {
            return AnimationSpawner.GetShakeAnimation(_textPivot, 0.5f);
        }
    }
}