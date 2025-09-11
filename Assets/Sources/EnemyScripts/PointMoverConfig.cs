using UnityEngine;

namespace Assets.Sources.EnemyScripts
{
    [CreateAssetMenu(menuName = "Configs/PointMoverConfig")]
    public class PointMoverConfig : ScriptableObject
    {
        public Vector3[] MovePoints;
        public float Speed;
        public bool IsRestart;
    }
}