using Pause;
using Utils;
using UnityEngine;

namespace UI.HealthUI
{
    public class PlayerHealthText : HealthText
    {
        [SerializeField] private PopUpText _popUpText;

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            _popUpText.Init(pauseHandler);
            MaxValue = 0;
            IsNeedToSplit = false;
            _popUpText.SetPreviousValue(Health.MaxValue);
            CurrentValue = Health.CurrentValue;
            Text.SetText(CurrentValue.ToString());
        }

        private protected override void OnUpdate()
        {
            base.OnUpdate();
            _popUpText.ShowText(Health.CurrentValue);
        }
    }
}