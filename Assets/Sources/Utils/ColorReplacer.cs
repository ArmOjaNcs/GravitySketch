using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.Utils
{
    public class ColorReplacer : MonoBehaviour
    {
        [Header("Available Colors")]
        [SerializeField] private List<Color> colors = new List<Color>();

        void Awake()
        {
            AssignRandomColors();
        }

        public void AssignRandomColors()
        {
            if (colors == null || colors.Count == 0)
            {
                Debug.LogWarning("Color list is empty.");
                return;
            }

            var renderers = GetComponentsInChildren<MeshRenderer>(true);

            foreach (var renderer in renderers)
            {
                var block = new MaterialPropertyBlock();
                renderer.GetPropertyBlock(block);
                block.SetColor(UserUtils.ColorID, colors[Random.Range(0, colors.Count)]);
                renderer.SetPropertyBlock(block);
            }
        }
    }
}