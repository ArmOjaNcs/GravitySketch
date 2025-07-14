using Assets.Sources.PlayerScripts;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Assets.Sources.Utils;
using Assets.Sources.Pause;

namespace Assets.Sources.UI
{
    public class UpgraderUI : PauseableObject
    {
        private const string MoveSpeed = "Move speed ";
        private const string BoostSpeed = "Boost speed ";
        private const string DefenceTime = "Defence time ";
        private const string Damage = "Damage ";
        private const string Upgraded = "Upgraded!!!";

        [SerializeField] private Upgrader _upgrader;
        [SerializeField] private SmoothedFade _smoothedFade;
        [SerializeField] private TextMeshProUGUI _moveSpeed;
        [SerializeField] private TextMeshProUGUI _boostSpeed;
        [SerializeField] private TextMeshProUGUI _defenceTime;
        [SerializeField] private TextMeshProUGUI _damage;
        [SerializeField] private TextMeshProUGUI _upgraded;
        [SerializeField] private RectTransform _textPivot;

        Tween _shakeAnimation;

        private void OnEnable()
        {
            _upgrader.Upgraded += OnUpgraded;
        }

        private void OnDisable()
        {
            _upgrader.Upgraded -= OnUpgraded;
        }

        private void Start()
        {
            _shakeAnimation = AnimationSpawner.GetShakeAnimation(_textPivot, 1);
            _upgraded.text = Upgraded;
            _upgraded.gameObject.SetActive(false);
            UpdateUI();
        }

        public override void Pause()
        {
            base.Pause();

            if(_shakeAnimation != null && _shakeAnimation.IsPlaying())
                _shakeAnimation.Pause();
        }

        public override void Resume()
        {
            base.Resume();

            if (_shakeAnimation != null && _shakeAnimation.IsComplete() == false)
                _shakeAnimation.Play();
        }

        private void OnUpgraded()
        {
            _smoothedFade.ShowElements();
            _shakeAnimation.Restart();
            _shakeAnimation.OnComplete(()=> _smoothedFade.FadeOut());
            UpdateUI();
        }

        private void UpdateUI()
        {
            _moveSpeed.text = MoveSpeed + _upgrader.MoveSpeed.ToString("F2");
            _boostSpeed.text = BoostSpeed + _upgrader.BoostSpeed.ToString("F2");
            _defenceTime.text = DefenceTime + _upgrader.DefendTime.ToString("F2");
            _damage.text = Damage + _upgrader.Damage.ToString("F2");
        }
    }
}