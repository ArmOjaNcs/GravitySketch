using Assets.Sources.Pause;
using Assets.Sources.Utils;
using UnityEngine;

namespace Assets.Sources.UI
{
    public class PlayerHealthText : HealthText
    {
        [SerializeField] private PopUpText _popUpText;

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            _popUpText.Init(pauseHandler);
            StartText = " ";
            SplitSign = UserUtils.DefaultChar;
            MaxValue = 0;
            IsNeedToSplit = false;
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