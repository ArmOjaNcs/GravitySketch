using Assets.Sources.PlayerScripts;
using TMPro;
using UnityEngine;

namespace Assets.Sources.UI
{
    public class BoostCountUI : MonoBehaviour
    {
        [SerializeField] private Booster _booster;
        [SerializeField] private TextMeshProUGUI _boostCount;

        private void OnEnable()
        {
            _booster.BoostCountChanged += OnBoostCountChanged;
        }

        private void OnDisable()
        {
            _booster.BoostCountChanged -= OnBoostCountChanged;
        }

        private void Start()
        {
            _boostCount.text = _booster.BoostCount.ToString();
        }

        private void OnBoostCountChanged()
        {
            _boostCount.text = _booster.CurrentBoostCount.ToString();
        }
    }
}