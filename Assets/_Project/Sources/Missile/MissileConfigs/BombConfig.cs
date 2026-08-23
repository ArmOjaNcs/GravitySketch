using UnityEngine;

namespace Missile.Configs
{
    [CreateAssetMenu(menuName = "Configs/BombConfig")]
    public class BombConfig : MissileConfig
    {
        public float ThrowForce;
        public Color WarningColor = Color.red;
        public float BlinkFrequency = 5f;
    }
}