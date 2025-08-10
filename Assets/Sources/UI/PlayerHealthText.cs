using Assets.Sources.Pause;
using Assets.Sources.Utils;
using UnityEngine;

namespace Assets.Sources.UI
{
    public class PlayerHealthText : HealthText
    {
        [SerializeField] private PopUpText _popUpText;

        private const string Energy = "Energy ";

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            _popUpText.Init(pauseHandler);
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