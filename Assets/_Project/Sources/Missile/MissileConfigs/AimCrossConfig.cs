using UnityEngine;

namespace Missile.Configs
{
    [CreateAssetMenu(menuName = "Configs/AimCrossConfig")]
    public class AimCrossConfig : MissileConfig
    {
        public float AimingTime;
        public float ShotDelay;
    }
}