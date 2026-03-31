using Assets.Sources.PlayerScripts;
using Assets.Sources.Utils;
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
            _booster.CountChanged += OnBoostCountChanged;
        }

        private void OnDisable()
        {
            _booster.CountChanged -= OnBoostCountChanged;
        }

        private void Start()
        {
            _boostCount.text = UserUtils.PlusSign.ToString();
        }

        private void OnBoostCountChanged()
        {
            if (_booster.CurrentBoostCount > 1)
                _boostCount.text = UserUtils.PlusSign.ToString();
            else
                _boostCount.text = "";
        }
    }
}