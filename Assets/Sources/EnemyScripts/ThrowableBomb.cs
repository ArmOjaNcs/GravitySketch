using UnityEngine;

namespace Assets.Sources.EnemyScripts
{
    public class ThrowableBomb : Bomb
    {
        public void AddForces(Vector3 position)
        {
            if (Rigidbody == null)
                return;

            Vector3 direction = (AttackZone.Player.Position - position).normalized;
            //direction.y = 0;
            Rigidbody.AddForce(direction * BombConfig.ThrowForce, ForceMode.Impulse);
            Rigidbody.AddRelativeTorque(Rigidbody.transform.forward * BombConfig.ThrowForce, ForceMode.Impulse);
        }
    }
}