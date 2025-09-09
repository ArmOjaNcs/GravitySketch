using UnityEngine;

namespace Assets.Sources.Table
{
    [RequireComponent(typeof(BoxCollider), typeof(MeshRenderer))]
    public class TemplateCube : MonoBehaviour, IReadonlyTemplateCube
    {
        [SerializeField] private MeshRenderer _meshRenderer;

        private bool _isInitiated;
        private bool _isColored;
        private Transform _transform;
        private static MaterialPropertyBlock _mpb;

        public bool IsMarked { get; private set; }
        public bool IsColored => _isColored;
        public CubeType Type { get; private set; }
        public int Index { get; private set; }
        public Vector3 Position => (_transform != null) ? _transform.position : transform.position;
        public Color Color { get; private set; }

        private void Awake()
        {
            _transform = transform;
            Color = _meshRenderer.material.color;

            if (_mpb == null)
                _mpb = new MaterialPropertyBlock();
        }

        public void Init(CubeType type, int index)
        {
            if (_isInitiated)
                return;

            Type = type;
            Index = index;
            _isInitiated = true;
        }

        public void SetColored(Color color)
        {
            if (_isColored)
                return;

            _isColored = true;
            Color = color;
            SetColor(color);
            EnableRendering();
        }

        public void DisableRendering()
        {
            SetColor(Color);

            if (_isColored == false)
                _meshRenderer.enabled = false;
        }

        public void EnableRendering() => _meshRenderer.enabled = true;

        public void Highlight(Color color) => SetColor(color);

        public void StopHighlight()
        {
            if (Type == CubeType.In)
                DisableRendering();
        }

        public void Mark() => IsMarked = true;

        private void SetColor(Color color)
        {
            if (_mpb == null)
                _mpb = new MaterialPropertyBlock();

            _mpb.SetColor("_Color", color);
            _meshRenderer.SetPropertyBlock(_mpb);
        }
    }
}