using Assets.Sources.Utils;
using TMPro;
using UnityEngine;

namespace Assets.Sources.UI
{
    public class EntryTranslator : MonoBehaviour
    {
        [SerializeField] private string _ru;
        [SerializeField] private string _en;
        [SerializeField] private string _tr;
        [SerializeField] private TextMeshProUGUI _text;

        private void Awake()
        {
            if (Translator.IsLangSetted)
                Translate();
            else
                Translator.LangChanged += OnLangChanged;
        }

        private void OnLangChanged()
        {
            Translator.LangChanged -= OnLangChanged;
            Translate();
        }

        private void Translate()
        {
            switch (Translator.CurrentLang)
            {
                case "ru":
                    _text.SetText(_ru);
                    break;

                case "en":
                    _text.SetText(_en);
                    break;

                case "tr":
                    _text.SetText(_tr);
                    break;
            }
        }
    }
}