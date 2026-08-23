using UnityEngine;

namespace Utils
{
    public class ModelColorizer : MonoBehaviour
    {
        [SerializeField] private Material _sharedMaterial;

        private void Awake()
        {
            MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>(true);

            foreach (MeshRenderer renderer in renderers)
            {
                Material[] materials = renderer.sharedMaterials;
                int count = materials.Length;

                for (int i = 0; i < count; i++)
                {
                    Color originalColor = materials[i].color;

                    materials[i] = _sharedMaterial;

                    MaterialPropertyBlock mpb = new MaterialPropertyBlock();
                    mpb.SetColor(UserUtils.ColorID, originalColor);

                    renderer.SetPropertyBlock(mpb, i);
                }

                renderer.sharedMaterials = materials;
            }
        }
    }
}