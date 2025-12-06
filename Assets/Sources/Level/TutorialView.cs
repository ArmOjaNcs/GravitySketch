using Assets.Sources.UI;
using UnityEngine;

namespace Assets.Sources.Level
{
    public class TutorialView : MenuWindow
    {
        [SerializeField] private TutorialType _type;
        [SerializeField] private ArrowUI[] _arrows;

        public TutorialType Type => _type;

        public override void Show()
        {
            base.Show();
            StartArrowAnimation();
        }

        private void StartArrowAnimation()
        {
            if (_arrows == null)
                return;

            foreach (var arrow in _arrows)
                arrow.Show();
        }
    }
}