using Assets.Sources.Dissolvable;
using Assets.Sources.Pause;
using Assets.Sources.Utils;
using DG.Tweening;
using UnityEngine;

namespace Assets.Sources.SimpleCubeScripts
{
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public class SimpleCube : DissolvableObject
    {
        [SerializeField] private MeshRenderer _meshRenderer;

        private Sequence _idleAnimation;
        private MaterialPropertyBlock _MPBlock;
        private Color _currentColor;

        public Color Color => _currentColor;

        private void OnEnable()
        {
            transform.parent = null;
        }

        private protected override void OnDisable()
        {
            base.OnDisable();

            if (_idleAnimation.IsActive())
                _idleAnimation.Kill();
        }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            _idleAnimation = AnimationSpawner.GetIdleAnimation(transform);
            _idleAnimation.Restart();
            IsInitialized = true;
        }

        public override void Pause()
        {
            base.Pause();

            if(_idleAnimation.IsActive())
                _idleAnimation.Pause();
        }

        public override void Resume()
        {
            base.Resume();

            if (_idleAnimation.IsActive())
                _idleAnimation.Play();
        }

        public void SetColor(Color color)
        {
            if (_MPBlock == null)
                _MPBlock = new MaterialPropertyBlock();

            _currentColor = color;
            _meshRenderer.GetPropertyBlock(_MPBlock);
            _MPBlock.SetColor("_Color", color);
            _meshRenderer.SetPropertyBlock(_MPBlock);
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