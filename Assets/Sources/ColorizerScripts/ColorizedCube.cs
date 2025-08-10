using Assets.Sources.Pause;
using Assets.Sources.Table;
using System;
using UnityEngine;

namespace Assets.Sources.ColorizerScripts
{
    [RequireComponent(typeof(MeshRenderer))]
    public class ColorizedCube : PauseableObject
    {
        private Vector3 _rotateDirection;
        private float _speed;
        private MeshRenderer _meshRenderer;
        private IReadonlyTemplateCube _target;
        private bool _isCanMove;
        private Transform _transform;

        private MaterialPropertyBlock _mpb;
        private Color _currentColor;

        public event Action<ColorizedCube> Finished;

        public bool IsAutoPaint { get; private set; }

        private void Update()
        {
            if (IsInitialized == false)
                return;

            if (_isCanMove && isActiveAndEnabled && IsPaused == false)
                MoveToTarget();
        }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            _meshRenderer = GetComponent<MeshRenderer>();
            _transform = transform;
            _mpb = new MaterialPropertyBlock();
            IsInitialized = true;
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

        public void StartMove() => _isCanMove = true;

        public int GetTargetIndex()
        {
            if (_target == null)
                throw new ArgumentNullException(nameof(_target));

            return _target.Index;
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