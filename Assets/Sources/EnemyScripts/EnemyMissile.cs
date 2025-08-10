using Assets.Sources.Audio;
using Assets.Sources.Pause;
using Assets.Sources.PlayerScripts;
using Assets.Sources.Utils;
using UnityEngine;

namespace Assets.Sources.EnemyScripts
{
    [RequireComponent(typeof(AudioPlayer))]
    public abstract class EnemyMissile : PauseableObject
    {
        private protected AudioPlayer AudioPlayer;
        private protected EnemyAttackZone AttackZone;
        private protected Transform Transform;
        private protected ParticleSystem Effect;
        private protected float CurrentLifeTime;
        private protected float CurrentEffectLifeTime;
        private protected float LifeTime;
        private protected float Radius;
        private protected float Damage;
        private protected float Force;
        private protected float Duration;
        private protected bool IsInteracted;
        private protected bool IsConfigurated;

        private protected virtual void OnEnable()
        {
            CurrentLifeTime = 0;
            CurrentEffectLifeTime = 0;
            IsInteracted = false;

            if (Effect != null)
            {
                Effect.transform.localPosition = Vector3.zero;
                Effect.Stop();
            }
        }

        private protected virtual void Update()
        {
            if (IsCanLive() == false)
                return;

            Live();

            if (IsInteracted)
                EndLife();
        }

        public virtual void InitFromConfig(MissileConfig missileConfig, EnemyAttackZone attackZone)
        {
            if (missileConfig == null)
            {
                Debug.LogError("MissileConfig is null in EnemyMissile.Initialize!");
                return;
            }

            if (IsConfigurated)
                return;

            AttackZone = attackZone;
            Transform = transform;
            LifeTime = missileConfig.LifeTime;
            Damage = missileConfig.Damage;
            Radius = missileConfig.Radius;
            Force = missileConfig.Force;
            Transform.localScale = missileConfig.Scale;
            Effect = Instantiate(missileConfig.Effect, transform).GetComponent<ParticleSystem>();
            Effect.transform.localPosition = Vector3.zero;
            Effect.Stop();
            Duration = Effect.main.duration;
        }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);

            if (IsInitialized)
                return;

            AudioPlayer = GetComponent<AudioPlayer>();
            AudioPlayer.Init(pauseHandler);
            AudioPlayer.AudioSource.playOnAwake = false;
            AudioPlayer.AudioSource.loop = false;
            AudioPlayer.AudioSource.spatialBlend = 1;
        }

        public override void Pause()
        {
            base.Pause();

            if (Effect != null && Effect.isPlaying)
                Effect.Pause();
        }

        public override void Resume()
        {
            base.Resume();

            if (Effect != null && IsInteracted)
                Effect.Play();
        }

        private protected virtual void Live()
        {
            if (CurrentLifeTime > LifeTime)
                return;

            CurrentLifeTime += Time.deltaTime;

            if (CurrentLifeTime > LifeTime)
                Interact();
        }

        private protected void EndLife()
        {
            if (CurrentEffectLifeTime > Duration)
                return;

            CurrentEffectLifeTime += Time.deltaTime;

            if (CurrentEffectLifeTime > Duration)
            {
                Effect.Stop();
                Effect.transform.SetParent(Transform);
                AttackZone.Return(gameObject);
            }
        }

        private protected virtual void Interact()
        {
            if (IsInteracted)
                return;

            IsInteracted = true;
            Collider[] hits = Physics.OverlapSphere(transform.position, Radius);

            foreach (Collider hit in hits)
            {
                if (hit.CompareTag(UserUtils.Player))
                {
                    Player player = AttackZone.Player;

                    if (player != null)
                        player.TakeDamage(Damage, transform.position, Force);

                    break;
                }
            }

            Effect.transform.SetParent(null);
            Effect.Play();
            AudioPlayer.Play();
        }

        private protected bool IsCanLive()
        {
            return IsInitialized && IsPaused == false && IsConfigurated;
        }
    }
}