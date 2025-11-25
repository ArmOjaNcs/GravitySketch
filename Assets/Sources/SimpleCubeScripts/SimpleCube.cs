using Assets.Sources.Dissolvable;
using Assets.Sources.Pause;
using UnityEngine;

namespace Assets.Sources.SimpleCubeScripts
{
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public class SimpleCube : DissolvableObject
    {
        [SerializeField] private MeshRenderer _meshRenderer;

        private MaterialPropertyBlock _MPBlock;
        private Color _currentColor;

        public Color Color => _currentColor;

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            transform.parent = null;
            IsInitialized = true;
        }

        public void SetColor(Color color)
        {
            if (_MPBlock == null)
                _MPBlock = new MaterialPropertyBlock();

            _currentColor = color;
            _meshRenderer.GetPropertyBlock(_MPBlock);
            _MPBlock.SetColor("_Color", color);
            _meshRenderer.SetPropertyBlock(_MPBlock);
        }
    }
}