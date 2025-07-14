using Assets.Sources.Pause;
using Assets.Sources.PlayerScripts;
using Assets.Sources.Utils;
using System.Collections;
using UnityEngine;

namespace Assets.Sources.EnemyScripts
{
    public abstract class EnemyMissile : PauseableRoutine
    {
        private protected EnemyAttackZone AttackZone;
        private protected Transform Transform;
        private protected ParticleSystem Effect;
        private protected float CurrentLifeTime;
        private protected float LifeTime;
        private protected float Radius;
        private protected float Damage;
        private protected float Force;
        private protected bool IsInteracted;

        public bool IsInitialized { get; protected set; }

        private protected virtual void OnEnable()
        {
            CurrentLifeTime = 0;
            IsInteracted = false;

            if (Effect != null)
            {
                Effect.transform.localPosition = Vector3.zero;
                Effect.Stop();

                if (Mathf.Approximately(Duration, 0))
                    Duration = Effect.main.duration;
            }
        }

        private protected virtual void Update()
        {
            if (IsInitialized == false || IsInteracted || IsPaused)
                return;

            Live();
        }

        public virtual void Initialize(MissileConfig missileConfig, EnemyAttackZone attackZone)
        {
            if (missileConfig == null)
            {
                Debug.LogError("MissileConfig is null in EnemyMissile.Initialize!");
                return;
            }

            if (IsInitialized)
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
        }

        public override void Pause()
        {
            base.Pause();

            if (Effect.isPlaying)
                Effect.Pause();
        }

        private protected virtual void Live()
        {
            CurrentLifeTime += Time.deltaTime;

            if (CurrentLifeTime > LifeTime)
                Interact();
        }

        private protected virtual void Interact()
        {
            if (IsInteracted)
                return;

            IsInteracted = true;
            OnUpdate();

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
        }

        private protected override IEnumerator UpdateRoutine(float duration)
        {
            float elapsedTime = 0;
            Effect.transform.SetParent(null);
            Effect.Play();

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;

                if (CurrentTime < elapsedTime)
                    CurrentTime = elapsedTime;

                yield return null;
            }

            Routine = null;
            CurrentTime = 0;
            Effect.Stop();
            Effect.transform.SetParent(Transform);
            AttackZone.Return(gameObject);
        }
    }
}