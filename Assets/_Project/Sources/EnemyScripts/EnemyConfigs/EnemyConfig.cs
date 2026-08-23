using UnityEngine;

namespace EnemyScripts.Configs
{
    [CreateAssetMenu(menuName = "Configs/EnemyConfig")]
    public class EnemyConfig : ScriptableObject
    {
        public int Level;
        public float Health;
        public string Name;
        public Vector3 MoveZoneScale;
        public Vector3 StopZoneScale;
        public Vector3 RetreatZoneScale;
        public Vector3 AttackZoneScale;

        [Header("Mover")]
        public float Speed;
        public float Acceleration;
        public Vector3 Scale;

        public EnemyAttackConfig AttackConfig;
    }
}