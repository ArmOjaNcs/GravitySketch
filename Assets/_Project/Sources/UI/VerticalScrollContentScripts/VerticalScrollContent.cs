using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.VerticalScrollContentScripts
{
    public class VerticalScrollContent : MonoBehaviour
    {
        [SerializeField] private RectTransform _content;
        [SerializeField] private LayoutGroup _layoutGroup;

        public event Action Rebuilded;

        public void Rebuild()
        {
            float height = 0;

            if (_layoutGroup is GridLayoutGroup grid)
                height = CalculateGridHeight(grid);
            else if (_layoutGroup is VerticalLayoutGroup vertical)
                height = CalculateVerticalHeight(vertical);
            else
                return;

            Vector2 size = _content.sizeDelta;
            size.y = height;
            _content.sizeDelta = size;

            Rebuilded?.Invoke();
        }

        private float CalculateVerticalHeight(VerticalLayoutGroup layout)
        {
            float height = layout.padding.top + layout.padding.bottom;
            int childCount = _content.childCount;

            for (int i = 0; i < childCount; i++)
            {
                RectTransform child = (RectTransform)_content.GetChild(i);
                height += GetChildHeight(child);

                if (i < childCount - 1)
                    height += layout.spacing;
            }

            return height;
        }

        private float CalculateGridHeight(GridLayoutGroup grid)
        {
            int childCount = _content.childCount;

            if (childCount == 0)
                return grid.padding.top + grid.padding.bottom;

            int columns = grid.constraint switch
            {
                GridLayoutGroup.Constraint.FixedColumnCount => grid.constraintCount,
                GridLayoutGroup.Constraint.FixedRowCount =>
                    Mathf.CeilToInt((float)childCount / grid.constraintCount),
                _ => 1
            };

            int rows = Mathf.CeilToInt((float)childCount / columns);

            return grid.padding.top +
                   grid.padding.bottom +
                   (rows * grid.cellSize.y) +
                   ((rows - 1) * grid.spacing.y);
        }

        private float GetChildHeight(RectTransform child)
        {
            LayoutElement element = child.GetComponent<LayoutElement>();

            if (element != null && element.preferredHeight > 0)
                return element.preferredHeight;

            return child.rect.height;
        }
    }
}