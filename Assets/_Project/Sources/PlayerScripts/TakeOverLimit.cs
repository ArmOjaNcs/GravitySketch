using System;
using Dissolvable;
using AnomalyScripts;
using EnemyScripts;
using Level;
using SimpleCubeScripts;
using Utils;
using UnityEngine;

namespace PlayerScripts
{
    [RequireComponent(typeof(BoxCollider))]
    public class TakeOverLimit : MonoBehaviour
    {
        [SerializeField] private Transform _hole;

        public event Action<SimpleCube> CubeAbsorbed;
        public event Action<int> Rewarded;
        public event Action EnemyDissolved;
        public event Action BarrierDissolved;
        public event Action<int> ObstacleDissolved;
        public event Action AnomalyDissolved;
        public event Action<float> MedAidAbsorbed;

        public int EnemiesDissolvedCount { get; private set; }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out SimpleCube simpleCube))
            {
                CubeAbsorbed?.Invoke(simpleCube);
            }
            else if (other.TryGetComponent(out Enemy enemy))
            {
                if (enemy.IsDissolving == false)
                {
                    enemy.Dissolve(_hole);
                    Rewarded?.Invoke(enemy.Reward);
                    EnemiesDissolvedCount++;
                    EnemyDissolved?.Invoke();
                }
            }
            else if (other.TryGetComponent(out MedAid medAid))
            {
                if (medAid.IsDissolving == false)
                {
                    MedAidAbsorbed?.Invoke(medAid.HealPower);
                    Rewarded?.Invoke(medAid.Reward);
                    medAid.Dissolve(_hole);
                }
            }
            else if (other.gameObject.CompareTag(UserUtils.Dropped) ||
                other.gameObject.CompareTag(UserUtils.DissolvableObstacle))
            {
                DissolvableObstacle dissolvableObstacle = other.transform.GetComponentInParent<DissolvableObstacle>();

                if (dissolvableObstacle != null && dissolvableObstacle.IsDissolving == false)
                {
                    dissolvableObstacle.Dissolve(_hole);
                    Rewarded?.Invoke(dissolvableObstacle.Reward);

                    if (other.gameObject.CompareTag(UserUtils.DissolvableObstacle))
                    {
                        BarrierDissolved?.Invoke();
                        return;
                    }

                    ObstacleDissolved?.Invoke(dissolvableObstacle.Size);

                    if (dissolvableObstacle.TryGetComponent(out Anomaly anomaly))
                        AnomalyDissolved?.Invoke();
                }
            }
            else if (other.gameObject.TryGetComponent(out LevelExit levelExit))
            {
                if (levelExit.IsDissolving == false)
                    levelExit.Dissolve(_hole);
            }
        }
    }
}