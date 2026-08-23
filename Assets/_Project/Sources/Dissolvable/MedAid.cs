using Dissolvable;
using Pause;
using Utils;
using UnityEngine;

namespace Dissolvable
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class MedAid : DissolvableObject
    {
        [SerializeField]
        [Min(0)] private float _healPower;

        public float HealPower => _healPower;

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            DropDown();
            gameObject.layer = UserUtils.NormalLayer;
            IsInitialized = true;
        }
    }
}