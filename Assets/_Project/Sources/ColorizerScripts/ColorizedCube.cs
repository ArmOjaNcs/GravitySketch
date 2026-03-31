using Assets.Sources.Pause;
using Assets.Sources.Table;
using System;
using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace Assets.Sources.ColorizerScripts
{
    [RequireComponent(typeof(MeshRenderer))]
    public class ColorizedCube : PauseableObject
    {
        [SerializeField] private ParticleSystem _effect;

        private MainModule _mainModule;
        private Vector3 _rotateDirection;
        private float _speed;
        private MeshRenderer _meshRenderer;
        private IReadonlyTemplateCube _target;
        private bool _isCanMove;
        private Transform _transform;

        private MaterialPropertyBlock _mpb;
        private Color _currentColor;

        public event Action<ColorizedCube> Finished;
        public event Action<ColorizedCube> EffectFinished;

        public bool IsAutoPaint { get; private set; }

        private void OnEnable()
        {
            if(_meshRenderer != null)
                _meshRenderer.enabled = true;
        }

        private void Update()
        {
            if (IsInitialized == false)
                return;

            if (isActiveAndEnabled && IsPaused == false)
            {
                if (_isCanMove)
                {
                    MoveToTarget();
                }
                else
                {
                    if (_effect.isPlaying == false)
                        EffectFinished?.Invoke(this);
                }    
            }
        }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            _meshRenderer = GetComponent<MeshRenderer>();
            _transform = transform;
            _mpb = new MaterialPropertyBlock();
            _mainModule = _effect.main;
            IsInitialized = true;
        }

        public override void Pause()
        {
            base.Pause();
            _effect.Pause();
        }

        public override void Resume()
        {
            base.Resume();
            _effect.Play();
        }

        public void SetStartSettings(ColorizedCubeData colorizedCubeData, bool isAutoPaint)
        {
            if (IsInitialized == false)
                return;

            _transform.position = colorizedCubeData.StartPosition;
            _target = colorizedCubeData.TemplateCube;
            _speed = colorizedCubeData.Speed;
            _rotateDirection = colorizedCubeData.RotateDirection;
            IsAutoPaint = isAutoPaint;

            _currentColor = colorizedCubeData.Color;
            _mpb.SetColor("_Color", _currentColor);
            _meshRenderer.SetPropertyBlock(_mpb);
        }

        public void StartMove()
        {
            _mainModule.startColor = _currentColor;
            _isCanMove = true;
        }

        public int GetTargetIndex()
        {
            if (_target == null)
                throw new ArgumentNullException(nameof(_target));

            return _target.Index;
        }

        public void DisableRenderer()
        {
            if (_meshRenderer != null)
                _meshRenderer.enabled = false;
        }

        public Color GetColor() => _currentColor;

        private void MoveToTarget()
        {
            _transform.position = Vector3.MoveTowards(_transform.position, _target.Position, _speed * Time.deltaTime);

            if (_transform.position == _target.Position)
            {
                _isCanMove = false;
                _target.SetColored(_currentColor);
                Finished?.Invoke(this);
            }

            Rotate();
        }

        private void Rotate() => _transform.Rotate(_rotateDirection * _speed * Time.deltaTime);
    }
}