using Assets.Sources.Table;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.Utils
{
    public class FenceColorizer : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _fence;
        [SerializeField] private TemplateColorReference _templateColorReference = null;

        private void Awake()
        {
            if(_templateColorReference != null)
                ColorizeFence(_templateColorReference);
        }

        public void ColorizeFence(TemplateColorReference templateColorReference)
        {
            foreach (var barrier in _fence)
            {
                MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
                materialPropertyBlock.SetColor("_Color", templateColorReference.GetRandomColor());
                barrier.GetComponent<MeshRenderer>()?.SetPropertyBlock(materialPropertyBlock);
            }
        }
    }
}