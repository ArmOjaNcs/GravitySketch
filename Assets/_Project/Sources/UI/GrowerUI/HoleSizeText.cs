using PlayerScripts;
using TMPro;
using UnityEngine;
using YG;

namespace UI.GrowerUI
{
    public class HoleSizeText : MonoBehaviour
    {
        [SerializeField] private GrowHandler _growHandler;
        [SerializeField] private TextMeshProUGUI _text;

        private string _size = string.Empty;

        private void OnEnable()
        {
            _growHandler.Growing += OnGrowing;
        }

        private void OnDisable()
        {
            _growHandler.Growing -= OnGrowing;
        }

        private void Start()
        {
            _size = _text.text + " ";
            OnGrowing();
        }

        private void OnGrowing()
        {
            _text.text = _size + _growHandler.CurrentSize;
        }
    }
}