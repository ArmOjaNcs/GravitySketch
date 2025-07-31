using Assets.Sources.PlayerScripts;
using TMPro;
using UnityEngine;

namespace Assets.Sources.UI
{
    public class BoostCountUI : MonoBehaviour
    {
        private const string Boosts = "Boost ";

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
            OnBoostCountChanged();
        }

        private void OnBoostCountChanged()
        {
            _boostCount.text = Boosts + _booster.CurrentBoostCount.ToString();
        }
    }
}