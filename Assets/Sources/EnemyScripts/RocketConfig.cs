using UnityEngine;

namespace Assets.Sources.EnemyScripts
{
    [CreateAssetMenu(menuName = "Configs/RocketConfig")]
    public class RocketConfig : MissileConfig
    {
        public float Speed;
        public float RotationSpeed = 5f;
        public float MaxTurnAngle = 45f;
        public float ReactionDelay = 0.3f;
    }
}