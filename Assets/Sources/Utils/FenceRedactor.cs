using Assets.Sources.Table;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.Utils
{
    public class FenceColorizer : MonoBehaviour
    {
        [SerializeField] private TemplateMaterialReference _materialReference;
        [SerializeField] private List<GameObject> _fence;

        private void Awake()
        {
            ColorizeFence();
        }

        private void ColorizeFence()
        {
            foreach (var barrier in _fence)
            {
                MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
                materialPropertyBlock.SetColor("_Color", _materialReference.GetRandomColor());
                barrier.GetComponent<MeshRenderer>().SetPropertyBlock(materialPropertyBlock);
            }
        }
    }
}