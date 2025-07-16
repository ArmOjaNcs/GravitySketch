using Assets.Sources.Utils;
using UnityEngine;

namespace Assets.Sources.UI
{
    public class PlayerHealthText : HealthText
    {
        [SerializeField] private PopUpText _popUpText;

        private const string Energy = "Energy ";

        private protected override void Start()
        {
            base.Start();
            StartText = Energy;
            IsNeedToSplit = true;
            SplitSign = '/';
            MaxValue = Health.MaxValue;
            _popUpText.SetPreviousValue(Health.MaxValue);
            CurrentValue = Health.CurrentValue;
            Text.text = GetTotalText();
        }

        private protected override void OnUpdate()
        {
            base.OnUpdate();
            _popUpText.ShowText(Health.CurrentValue);
        }
    }
}