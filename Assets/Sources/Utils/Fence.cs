using UnityEngine;

namespace Assets.Sources.Utils
{
    public class Fence : MonoBehaviour
    {
        public void SetColor(Color color)
        {
            MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
            materialPropertyBlock.SetColor("_Color", color);
            GetComponent<MeshRenderer>().SetPropertyBlock(materialPropertyBlock);
        }
    }
}