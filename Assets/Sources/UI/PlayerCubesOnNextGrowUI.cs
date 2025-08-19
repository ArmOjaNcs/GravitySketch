using Assets.Sources.PlayerScripts;
using TMPro;
using UnityEngine;

namespace Assets.Sources.UI
{
    public class PlayerCubesOnNextGrowUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private GrowHandler _growHandler;

        private string _cubesOnNextSize = string.Empty;

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
            _cubesOnNextSize = _text.text + " ";
            OnGrowing();
        }

        private void OnGrowing()
        {
            _text.text = _cubesOnNextSize + _growHandler.CubesOnNextGrow;
        }
    }
}