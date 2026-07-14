using Assets.Sources.Pause;
using Assets.Sources.PlayerScripts;
using Assets.Sources.Utils;
using UnityEngine;

namespace Assets.Sources.Table
{
    public class HoleMaskHandler : PauseableRoutine
    {
        [SerializeField] private Mover _mover;
        [SerializeField] private Grower _grower;

        private Renderer _renderer;
        private Material _material;
        private Transform _transform;
        private float _targetRadius;
        private float _currentRadius;

        private void OnEnable()
        {
            _mover.PositionChanged += OnPositionChanged;
            _grower.SizeChanged += OnSizeChanged;
        }

        private protected override void OnDisable()
        {
            base.OnDisable();
            _mover.PositionChanged -= OnPositionChanged;
            _grower.SizeChanged -= OnSizeChanged;
        }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            _targetRadius = UserUtils.GetCorrectRadius(_mover.transform.lossyScale.x);
            _currentRadius = 0;
            _transform = transform;
        }

        public void Init(PauseHandler pauseHandler, Renderer renderer, Material material)
        {
            Init(pauseHandler);
            _material = material;
            _material.SetFloat("_HoleRadius", _currentRadius);
            _renderer = renderer;
            _renderer.material = _material;
            _transform = transform;
            IsInitialized = true;
        }

        private protected override void OnRoutineIteration(float cycleDuration) 
        {
            float progress = ElapsedTime / cycleDuration;
            _currentRadius = Mathf.Lerp(_currentRadius, _targetRadius, progress);
            _material.SetFloat("_HoleRadius", _currentRadius);
        }

        private protected override void OnRoutineEnd()
        {
            base.OnRoutineEnd();
            _currentRadius = _targetRadius;
        }

        private void OnPositionChanged(Vector3 position)
        {
            _material.SetVector("_HolePosition", new Vector4(position.x, _transform.position.y, position.z, 0));
        }

        private void OnSizeChanged(float sizeDelta)
        {
            _targetRadius = UserUtils.GetCorrectRadius(sizeDelta);
            UpdateView(Duration);
        }
    }
}