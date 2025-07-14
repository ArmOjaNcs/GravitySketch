using Assets.Sources.Utils;
using System.Collections;
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
        private Vector3 _defaultScale;
        private Vector3 _defaultEffectScale;
        private Vector3 _initialScale;
        private float _currentAimingTime;
        private float _currentDelayTime;
        private AimCrossConfig _config;
        private bool _isShoot;

        private Vector3 TargetScale => _initialScale * UserUtils.HalfUnit;
        public bool IsAiming {  get; private set; }

        private protected override void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _defaultScale = _rectTransform.lossyScale;
            base.Awake();
        }

        private protected override void Start()
        {
            base.Start();

            if (Effect != null)
            {
                _defaultEffectScale = Effect.transform.lossyScale;
                Debug.Log($"defScale {_defaultEffectScale}");
            }
        }

        private protected override void OnEnable()
        {
            base.OnEnable();
            _rectTransform.SetParent(null);
            _rectTransform.localScale = _defaultScale;
            _image.color = Color.clear;
            _currentAimingTime = 0;
        }

        public override void Initialize(MissileConfig config, EnemyAttackZone attackZone)
        {
            base.Initialize(config, attackZone);
            
            _config = config.SafeCast<AimCrossConfig>();
            if (_config != null)
            {
            Debug.Log("Initialized");
                IsInitialized = true;
                return;
            }

            IsInitialized = false;
        }

        public void StartAimWarning()
        {
            if (IsAiming || _isShoot)
                return;

            OnEnable();
            _rectTransform.localScale = Vector3.one;

            _initialScale = new Vector3(
                _defaultScale.x / transform.lossyScale.x * AttackZone.Player.transform.lossyScale.x,
                _defaultScale.y / transform.lossyScale.y * AttackZone.Player.transform.lossyScale.y,
                _defaultScale.z / transform.lossyScale.z * AttackZone.Player.transform.lossyScale.z
                );

            _rectTransform.localScale = _initialScale;

            IsAiming = true;
        }

        private protected override IEnumerator UpdateRoutine(float duration)
        {
            Effect.transform.SetParent(null);
            Effect.transform.localScale = _defaultEffectScale;
            Effect.transform.rotation = Quaternion.identity;
            return base.UpdateRoutine(duration);
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

                if(_currentDelayTime > _config.ShotDelay)
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