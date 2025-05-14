using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ColorizerView : MonoBehaviour
{
    [SerializeField] private Image[] _colors;
    [SerializeField] private Colorizer _colorizer;

    private void OnEnable()
    {
        _colorizer.QueueChanged += OnQueueChanged;
    }

    private void OnDisable()
    {
        _colorizer.QueueChanged -= OnQueueChanged;
    }

    private void OnQueueChanged(IEnumerable<Color> colors)
    {
        Debug.Log("ColorsCount" + colors.Count());
        if (colors.Count() >= _colors.Length)
        {
            for (int i = 0; i < _colors.Length; i++)
                _colors[i].color = colors.ElementAt(i);
        }
        else
        {
            for (int i = 0; i < colors.Count(); i++)
            {
                if (_colors[i].isActiveAndEnabled)
                    _colors[i].color = colors.ElementAt(i);
            }

            _colors[colors.Count()].gameObject.SetActive(false);
        }
    }
}