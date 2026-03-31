using Assets.Sources.UI;
using UnityEngine;

namespace Assets.Sources.Level
{
    public class TutorialView : MenuWindow
    {
        [SerializeField, Min(0)] private int _index;
        [SerializeField] private ArrowUI[] _arrows;

        public int Index => _index;

        private void StartArrowAnimation()
        {
            if (_arrows.Length == 0)
                return;

            foreach (var arrow in _arrows)
                arrow.Show();
        }

        private void HideArrow()
        {
            if (_arrows.Length == 0)
                return;

            foreach (var arrow in _arrows)
                arrow.gameObject.SetActive(false);
        }

        private protected override void OnBackClicked()
        {
            HideArrow();
            base.OnBackClicked();
        }

        private protected override void OnOpened()
        {
            base.OnOpened();
            StartArrowAnimation();
        }
    }
}