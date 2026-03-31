using UnityEngine;

namespace Assets.Sources.Table
{
    [RequireComponent(typeof(BoxCollider))]
    public class Barrier : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private BoxCollider _collider;

        public MeshRenderer MeshRenderer => _meshRenderer;
        public BoxCollider Collider => _collider;

        public void SetColor(Color color)
        {
            MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
            materialPropertyBlock.SetColor("_Color", color);
            _meshRenderer.SetPropertyBlock(materialPropertyBlock);
        }
    }
}