using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ColorizerView : MonoBehaviour
{
    [SerializeField] private Image[] _colors;
    [SerializeField] private ColoringPositionHandler _coloringPositionHandler;

    private void OnEnable()
    {
        _coloringPositionHandler.QueueChanged += OnQueueChanged;
    }

    private void OnDisable()
    {
        _coloringPositionHandler.QueueChanged -= OnQueueChanged;
    }

    private void OnQueueChanged(IEnumerable<Color> colors)
    {
        if (colors.Count() >= _colors.Length)
        {
            for (int i = 0; i < _colors.Length; i++)
                _colors[i].color = colors.ElementAt(i);
        }
        else
        {
            for (int i = 0; i < colors.Count(); i++)
            {
                _colors[i].color = colors.ElementAt(i);
                _colors[_colors.Count() - (i + 1)].gameObject.SetActive(false);
            }
        }
    }
}