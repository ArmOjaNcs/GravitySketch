using EnemyScripts;
using Pause;
using Utils;
using TMPro;
using UnityEngine;

namespace UI.HealthUI
{
    public class EnemyText : HealthText
    {
        private const string Level = nameof(Level);

        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private Enemy _enemy;

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            IsNeedToSplit = true;
            SplitSign = '/';
            MaxValue = Health.MaxValue;
            CurrentValue = Health.CurrentValue;
            Text.SetText(CurrentValue.ToString() + SplitSign + MaxValue);
            string levelTranslation = Translator.Get(Level);
            _levelText.SetText(levelTranslation + " " + _enemy.Size);
            _nameText.SetText(Translator.Get(_enemy.Name));
            IsInitialized = true;
        }
    }
}