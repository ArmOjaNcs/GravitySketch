using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.ColorizerScripts
{
    public class ColorData : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private Color _color;
        private int _count;

        public event Action<ColorData> Selected;

        public event Action Deselected;

        public event Action CountChanged;

        public event Action Initialized;

        public Color Color => _color;

        public int Count => _count;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClicked);
        }

        public void Init(Color color, int count)
        {
            _color = color;
            _count = count;
            Initialized?.Invoke();
        }

        public void ReduceCount()
        {
            _count--;
            CountChanged?.Invoke();
        }

        public void Select() => Selected?.Invoke(this);

        public void Deselect() => Deselected.Invoke();

        public void SwitchButtonInteraction(bool interactionValue)
        {
            _button.interactable = interactionValue;
            _button.image.raycastTarget = interactionValue;
        }

        private void OnButtonClicked() => Selected?.Invoke(this);
    }
}