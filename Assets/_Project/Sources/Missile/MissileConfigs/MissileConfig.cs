using UnityEngine;

namespace Missile.Configs
{
    public abstract class MissileConfig : ScriptableObject
    {
        public float LifeTime;
        public float Damage;
        public float Radius;
        public Color Color;
        public Vector3 Scale;
        public GameObject Effect;
    }
}