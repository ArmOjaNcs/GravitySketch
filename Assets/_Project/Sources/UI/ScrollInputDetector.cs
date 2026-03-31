using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Sources.UI
{
    public class ScrollInputDetector : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, 
        IPointerDownHandler, IPointerUpHandler 
    {
        private readonly List<RaycastResult> _raycastResults = new();
        private bool _isDragging;
        private bool _isPointerDowned;
        private PointerEventData _eventData;

        public event Action<float> ScrolledByWheel;
        public event Action<PointerEventData> BeginDrag;
        public event Action<PointerEventData> Dragging;
        public event Action<PointerEventData> EndDrag;
        public event Action Interacted;
        public event Action InteractStopped;

        private void Awake()
        {
            _eventData = new PointerEventData(EventSystem.current);
        }

        private void OnDisable()
        {
            _isPointerDowned = false;
            _isDragging = false;
            InteractStopped?.Invoke();
        }

        private void Update()
        {
            if (_isDragging)
                return;

            if (IsPointerOverViewport() == false)
                return;

            float wheel = Input.mouseScrollDelta.y;

            if (Mathf.Abs(wheel) < 0.01f)
            {
                if(_isPointerDowned == false)
                    InteractStopped?.Invoke();

                return;
            }

            ScrolledByWheel?.Invoke(wheel);
            Interacted?.Invoke();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _isDragging = true;
            Interacted?.Invoke();
            BeginDrag?.Invoke(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Dragging?.Invoke(eventData);
            Interacted?.Invoke();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _isDragging = false;
            EndDrag?.Invoke(eventData);

            if(_isPointerDowned == false)
                InteractStopped?.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _isPointerDowned = true;
            Interacted?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isPointerDowned = false;

            if(_isDragging == false)
                InteractStopped?.Invoke();
        }

        private bool IsPointerOverViewport()
        {
            _eventData.position = Input.mousePosition;
            _raycastResults.Clear();

            EventSystem.current.RaycastAll(_eventData, _raycastResults);

            foreach (var result in _raycastResults)
            {
                if (result.gameObject == gameObject)
                    return true;
            }

            return false;
        }
    }
}