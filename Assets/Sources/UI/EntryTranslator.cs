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

        private void Start()
        {
            switch (Translator.CurrentLang)
            {
                case "ru":
                    _text.text = _ru;
                    break;

                case "en":
                    _text.text = _en;
                    break;

                case "tr":
                    _text.text = _tr;
                    break;
            }
        }
    }
}