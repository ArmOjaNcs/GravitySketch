using Assets.Sources.Pause;
using Assets.Sources.PlayerScripts;
using UnityEngine;

namespace Assets.Sources.Table
{
    public class HoleMaskHandler : PauseableRoutine
    {
        [SerializeField] private Mover _mover;
        [SerializeField] private Material _material;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Grower _grower;
        [SerializeField] private Player _player;

        private Transform _transform;
        private float _targetRadius;
        private float _currentRadius;
        
        private protected override void Awake()
        {
            base.Awake();
            _targetRadius = _mover.transform.lossyScale.x / 2;
            _material.SetFloat("_HoleRadius", _targetRadius);
            _currentRadius = _targetRadius;
            _transform = transform;
            _renderer.material = _material;
        }

        private void OnEnable()
        {
            _mover.PositionChanged += OnPositionChanged;
            _grower.SizeChanged += OnSizeChanged;
            _player.IsDead += OnPlayerDead;
        }

        private protected override void OnDisable()
        {
            base.OnDisable();
            _mover.PositionChanged -= OnPositionChanged;
            _grower.SizeChanged -= OnSizeChanged;
            _player.IsDead -= OnPlayerDead;
        }

        private void OnPositionChanged(Vector3 position)
        {
            _material.SetVector("_HolePosition", new Vector4(position.x, _transform.position.y, position.z, 0));
        }

        private void OnSizeChanged(float sizeDelta)
        {
            _targetRadius += sizeDelta / 2;
            UpdateView(Duration);
        }

        private void OnPlayerDead()
        {
            _targetRadius = 0;
            UpdateView(Duration);
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
    }
}