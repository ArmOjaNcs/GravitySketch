using UnityEngine;

namespace AnomalyScripts
{
    [CreateAssetMenu(menuName = "Configs/AnomalyConfig")]
    public class AnomalyConfig : ScriptableObject
    {
        public Vector3 StartPosition;
        public Vector3 Scale;
        public int Size;
        public PointMoverConfig PointMoverConfig;
    }
}