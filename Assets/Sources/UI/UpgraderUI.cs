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
        [SerializeField] private TextMeshProUGUI _defenceTime;
        [SerializeField] private TextMeshProUGUI _defence;
        [SerializeField] private TextMeshProUGUI _damage;
        [SerializeField] private TextMeshProUGUI _upgraded;
        [SerializeField] private RectTransform _textPivot;

        private string _moveSpeedText = string.Empty;
        private string _defenceTimeText = string.Empty;
        private string _defenceValueText = string.Empty;
        private string _damageText = string.Empty;

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
            _upgraded.gameObject.SetActive(false);
            _moveSpeedText = _moveSpeed.text + " ";
            _defenceTimeText = _defenceTime.text + " ";
            _defenceValueText = _defence.text + " ";
            _damageText = _damage.text + " ";
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
            _moveSpeed.text = _moveSpeedText + _upgrader.MoveSpeed.ToString("F1");
            _defenceTime.text = _defenceTimeText + _upgrader.DefendTime.ToString("F1");
            float defencePercent = _upgrader.Defence * 10;
            defencePercent = Mathf.Clamp(defencePercent, 0, 75);
            _defence.text = _defenceValueText + defencePercent.ToString() + '%';
            float damagePerSecond = _upgrader.Damage * 2;
            _damage.text = _damageText + damagePerSecond.ToString();
        }

        private protected override Sequence GetAnimation()
        {
            return AnimationSpawner.GetShakeAnimation(_textPivot, 0.5f);
        }
    }
}