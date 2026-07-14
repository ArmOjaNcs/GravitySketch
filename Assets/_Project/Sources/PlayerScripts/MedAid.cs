using Assets.Sources.Dissolvable;
using Assets.Sources.Pause;
using Assets.Sources.Utils;
using UnityEngine;

namespace Assets.Sources.PlayerScripts
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