using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Sources.Table
{
    [CreateAssetMenu(fileName = "TemplateColorReference", menuName = "ScriptableObjects/TemplateColorReference")]
    public class TemplateColorReference : ScriptableObject
    {
        [Serializable]
        private class ColorEntry
        {
            [SerializeField] private List<int> _indexes = new();
            [SerializeField] private Color _color;
            [SerializeField] private int _count;

            private int _currentIndex;

            public Color Color => _color;
            public int Count => _count;
            public bool HasFreeIndex => _currentIndex < _indexes.Count;

            public bool HasIndex(int index) => _indexes.Contains(index);
            public void AddIndex(int index) => _indexes.Add(index);
            public void IncrementCount() => _count++;
            public void SetColor(Color color) => _color = color;
            public bool TryGetIndex(out int index)
            {
                if (_currentIndex < _indexes.Count)
                {
                    index = _indexes[_currentIndex];
                    _currentIndex++;
                    return true;
                }

                index = -1;
                return false;
            }

            public void ResetCurrentIndex()
            {
                _currentIndex = 0;
            }
        }

        [SerializeField] private List<ColorEntry> _entries = new();

        public int GetTotalCount()
        {
            int count = 0;

            foreach (ColorEntry colorEntry in _entries)
                count += colorEntry.Count;

            return count;
        }

        public void AddColorEntry(Color color, int index)
        {
            ColorEntry existing = _entries.Find(e => e.Color == color);

            if (existing != null)
            {
                existing.IncrementCount();
                existing.AddIndex(index);
            }
            else
            {
                ColorEntry newEntry = new ColorEntry();
                newEntry.SetColor(color);
                newEntry.AddIndex(index);
                newEntry.IncrementCount();
                _entries.Add(newEntry);
            }
        }

        public void HighlightAllCubes(IReadOnlyList<IReadonlyTemplateCube> templateCubes)
        {
            foreach (TemplateCube cube in templateCubes)
            {
                int index = cube.Index;
                
                foreach (ColorEntry entry in _entries)
                {       
                    if (entry.HasIndex(index))
                    {
                        Debug.Log($"index in entries");
                        cube.EnableRendering();
                        cube.Highlight(entry.Color);
                        break;
                    }
                }
            }
        }

        public void ResetEntriesCurrentIndex()
        {
            foreach (ColorEntry entry in _entries)
                entry.ResetCurrentIndex();
        }

        public List<Color> GetAllColors()
        {
            List<Color> result = new();

            foreach (var entry in _entries)
            {
                for (int i = 0; i < entry.Count; i++)
                    result.Add(entry.Color);
            }

            return result;
        }

        public Color GetColor(int index)
        {
            foreach (var entry in _entries)
            {
                if (entry.HasIndex(index))
                    return entry.Color;
            }

            return Color.white;
        }

        public bool HasFreeIndex(Color color)
        {
            foreach (var entry in _entries)
            {
                if (entry.Color == color && entry.HasFreeIndex)
                    return true;
            }

            return false;
        }

        public bool TryGetIndexByColor(Color color, out int index)
        {
            foreach (var entry in _entries)
            {
                if (entry.Color == color && entry.HasFreeIndex)
                {
                    if (entry.TryGetIndex(out index))
                        return true;
                }
            }

            index = 0;
            return false;
        }

        public Color GetRandomColor()
        {
            int randomIndex = Random.Range(0, _entries.Count);

            return _entries[randomIndex].Color;
        }
    }
}