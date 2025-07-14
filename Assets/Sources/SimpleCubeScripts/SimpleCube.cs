using Assets.Sources.Dissolvable;
using Assets.Sources.Utils;
using DG.Tweening;
using UnityEngine;

namespace Assets.Sources.SimpleCubeScripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class SimpleCube : DissolvableObject
    {
        [SerializeField] private MeshRenderer _meshRenderer;

        private Sequence _idleAnimation;
        private MaterialPropertyBlock _mpb;
        private Color _currentColor;

        public Color Color => _currentColor;

        private void Awake()
        {
            _idleAnimation = AnimationSpawner.GetIdleAnimation(transform);
            _idleAnimation.Restart();
        }

        private void OnEnable()
        {
            transform.parent = null;
        }

        public void SetColor(Color color)
        {
            if (_mpb == null)
                _mpb = new MaterialPropertyBlock();

            _currentColor = color;
            _meshRenderer.GetPropertyBlock(_mpb);
            _mpb.SetColor("_Color", color);
            _meshRenderer.SetPropertyBlock(_mpb);
            Debug.Log("colorSeted");
        }

        public override void DropDown()
        {
            base.DropDown();

            _idleAnimation.Pause();
            _idleAnimation.Kill();
        }
    }
}