using System;
using UnityEngine;
using Assets.Sources.Utils;
using Assets.Sources.EnemyScripts;
using Assets.Sources.SimpleCubeScripts;
using Assets.Sources.Dissolvable;
using Assets.Sources.Level;

namespace Assets.Sources.PlayerScripts
{
    [RequireComponent(typeof(BoxCollider))]
    public class TakeOverLimit : MonoBehaviour
    {
        [SerializeField] private Transform _hole;

        public event Action<SimpleCube> CubeAbsorbed;
        public event Action<int> Rewarded;
        public event Action EnemyDissolved;
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
            else if(other.TryGetComponent(out MedAid medAid))
            {
                if(medAid.IsDissolving == false)
                {
                    MedAidAbsorbed?.Invoke(medAid.HealPower);
                    Rewarded?.Invoke(medAid.Reward);
                    medAid.Dissolve(_hole);
                }
            }
            else if (other.gameObject.CompareTag(UserUtils.Dissolved))
            {
                DissolvableObstacle dissolvableObstacle = other.transform.GetComponentInParent<DissolvableObstacle>();

                if (dissolvableObstacle != null && dissolvableObstacle.IsDissolving == false)
                {
                    dissolvableObstacle.Dissolve(_hole);
                    Rewarded?.Invoke(dissolvableObstacle.Reward);
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