using UnityEngine;
using UnityEngine.EventSystems;

namespace Utils
{
    public class SinglePressButton : MonoBehaviour, IPointerClickHandler
    {
        public bool IsPressed { get; private set; }

        public void OnPointerClick(PointerEventData eventData)
        {
            IsPressed = true;
        }

        private void LateUpdate()
        {
            if (IsPressed)
                IsPressed = false;
        }
    }
}