using Assets.Sources.PlayerScripts;
using TMPro;
using UnityEngine;

namespace Assets.Sources.UI
{
    public class HoleSizeText : MonoBehaviour
    {
        private const string Size = "Size: ";

        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private GrowHandler _growHandler;

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
            OnGrowing();
        }

        private void OnGrowing()
        {
            _text.text = Size + _growHandler.CurrentSize;
        }
    }
}