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
                MeshRenderer[] renderers = barrier.GetComponentsInChildren<MeshRenderer>(true);
                MaterialPropertyBlock mpb = new MaterialPropertyBlock();

                foreach (MeshRenderer renderer in renderers)
                {
                    int materialCount = renderer.sharedMaterials.Length;

                    for (int materialIndex = 0; materialIndex < materialCount; materialIndex++)
                    {
                        mpb.Clear();
                        mpb.SetColor(UserUtils.ColorID, UserUtils.GetRandomColor());
                        renderer.SetPropertyBlock(mpb, materialIndex);
                    }
                }
            }
        }
    }
}