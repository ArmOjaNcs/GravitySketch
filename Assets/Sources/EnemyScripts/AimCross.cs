using Assets.Sources.Pause;
using Assets.Sources.Utils;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.EnemyScripts
{
    [RequireComponent(typeof(RectTransform))]
    public class AimCross : EnemyMissile
    {
        [SerializeField] private Image _image;
        [SerializeField] private Color _startColor;
        [SerializeField] private Color _endColor;

        private RectTransform _rectTransform;
        private Transform _playerTransform;
        private Vector3 _defaultScale;
        private Vector3 _defaultEffectScale;
        private Vector3 _initialScale;
        private float _currentAimingTime;
        private float _currentDelayTime;
        private AimCrossConfig _config;
        private bool _isShoot;

        public event Action Shoot;

        private Vector3 TargetScale => _initialScale * UserUtils.HalfUnit;
        public bool IsAiming { get; private set; }

        private protected override void OnEnable()
        {
            if (IsInitialized == false)
                return;

            base.OnEnable();
            _rectTransform.SetParent(null);
            _rectTransform.localScale = _defaultScale;
            _image.color = Color.clear;
            _currentAimingTime = 0;
        }

        public override void InitFromConfig(MissileConfig config, EnemyAttackZone attackZone)
        {
            base.InitFromConfig(config, attackZone);

            _config = config.SafeCast<AimCrossConfig>();

            if (Effect != null)
                _defaultEffectScale = Effect.transform.lossyScale;

            if (_config != null)
            {
                _defaultEffectScale = Effect.transform.lossyScale;
                IsConfigurated = true;
                return;
            }

            IsConfigurated = false;
        }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            _rectTransform = GetComponent<RectTransform>();
            _defaultScale = Vector3.one * UserUtils.ThirdOfUnit;
            IsInitialized = true;
            OnEnable();
        }

        public void StartAimWarning()
        {
            if (IsAiming || _isShoot)
                return;

            if (_playerTransform == null)
                _playerTransform = AttackZone.Player.transform;

            OnEnable();
            _rectTransform.localScale = Vector3.one;

            _initialScale = new Vector3(
                _defaultScale.x / transform.lossyScale.x * _playerTransform.lossyScale.x,
                _defaultScale.y / transform.lossyScale.y * _playerTransform.lossyScale.y,
                _defaultScale.z / transform.lossyScale.z * _playerTransform.lossyScale.z
                );

            _rectTransform.localScale = _initialScale;

            IsAiming = true;
        }

        private protected override void Interact()
        {
            if (IsInteracted)
                return;

            IsInteracted = true;

            Shoot?.Invoke();
            Effect.transform.SetParent(null);

            if (IsHitPlayer())
            {
                if (AttackZone.Player.IsDefended)
                    Effect.transform.position += new Vector3(0, AttackZone.Player.Radius, 0);
                else
                    AttackZone.Player.TakeDamage(Damage, transform.position, Force);
            }

            Effect.transform.localScale = _defaultEffectScale;
            Effect.transform.rotation = Quaternion.identity;
            PlayEffect();
        }

        private protected override void Live()
        {
            if (IsAiming)
            {
                _currentAimingTime += Time.deltaTime;
                float progress = _currentAimingTime / _config.AimingTime;
                _rectTransform.position = AttackZone.Player.Position + Vector3.up * 0.1f;
                _image.color = Color.Lerp(_startColor, _endColor, progress);
                _rectTransform.localScale = Vector3.Lerp(_initialScale, TargetScale, progress);

                if (_currentAimingTime > _config.AimingTime)
                {
                    IsAiming = false;
                    _image.color = Color.red;
                    _currentAimingTime = 0;
                    _rectTransform.position = _rectTransform.position;
                    _isShoot = true;
                }
            }

            if (_isShoot)
            {
                _currentDelayTime += Time.deltaTime;

                if (_currentDelayTime > _config.ShotDelay)
                {
                    _currentDelayTime = 0;
                    _isShoot = false;
                    _image.color = Color.clear;
                    Interact();
                }
            }
        }
    }
}