using UnityEngine;

namespace Assets.Sources.EnemyScripts
{
    [CreateAssetMenu(menuName = "Configs/AimCrossConfig")]
    public class AimCrossConfig : MissileConfig
    {
        public float AimingTime;
        public float ShotDelay;
    }
}