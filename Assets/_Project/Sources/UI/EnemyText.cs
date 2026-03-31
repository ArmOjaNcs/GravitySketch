using Assets.Sources.EnemyScripts;
using Assets.Sources.Pause;
using Assets.Sources.Utils;
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

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            IsNeedToSplit = true;
            SplitSign = '/';
            MaxValue = Health.MaxValue;
            CurrentValue = Health.CurrentValue;
            Text.text = GetTotalText();
            string levelTranslation = Translator.Get(Level);
            _levelText.text = levelTranslation + " " + _enemy.Size;
            _nameText.text = Translator.Get(_enemy.Name);
            IsInitialized = true;
        }
    }
}