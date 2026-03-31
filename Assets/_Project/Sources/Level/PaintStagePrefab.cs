using Assets.Sources.Table;
using UnityEngine;

namespace Assets.Sources.Level
{
    public class PaintStagePrefab : MonoBehaviour
    {
        [SerializeField] private Template _template;
        [SerializeField] private TemplateColorReference _colorReference;
        [SerializeField] private Material _tableMaterial;

        public Template Template => _template;
        public TemplateColorReference ColorReference => _colorReference;
        public Material TableMaterial => _tableMaterial;
    }
}