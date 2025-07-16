using Assets.Sources.EnemyScripts;
using TMPro;
using UnityEngine;

namespace Assets.Sources.UI
{
    public class EnemyText : HealthText
    {
        private const string Level = nameof(Level);

        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private Enemy _enemy;

        private protected override void Start()
        {
            base.Start();
            IsNeedToSplit = true;
            SplitSign = '/';
            MaxValue = Health.MaxValue;
            CurrentValue = Health.CurrentValue;
            Text.text = GetTotalText();
            _levelText.text = Level + " " + _enemy.Size;
            _nameText.text = _enemy.Name;
        }
    }
}