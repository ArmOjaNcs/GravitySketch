using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class FenceColorizer : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _fence;

        private void Start()
        {
            ColorizeFence();
        }

        public void ColorizeFence()
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