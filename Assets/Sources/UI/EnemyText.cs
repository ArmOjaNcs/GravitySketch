using Assets.Sources.EnemyScripts;
using Assets.Sources.Pause;
using TMPro;
using UnityEngine;
using YG;

namespace Assets.Sources.UI
{
    public class EnemyText : HealthText
    {
        private const string Level = nameof(Level);

        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private Enemy _enemy;

        private LanguageYG _levelTextLanguage;
        private LanguageYG _nameTextLanguage;

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            IsNeedToSplit = true;
            SplitSign = '/';
            MaxValue = Health.MaxValue;
            CurrentValue = Health.CurrentValue;
            Text.text = GetTotalText();
            _levelText.text = Level + " " + _enemy.Size;
            _nameText.text = _enemy.Name;
            IsInitialized = true;
            //_levelTextLanguage = _levelText.gameObject.GetComponent<LanguageYG>();
            //_nameTextLanguage = _nameText.gameObject.GetComponent<LanguageYG>();
            //_levelTextLanguage.text = _levelText.text;
            //_levelTextLanguage.Translate(3);
            //_nameTextLanguage.text = _nameText.text;
            //_nameTextLanguage.Translate(3);
        }
    }
}