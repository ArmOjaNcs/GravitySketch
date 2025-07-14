using Assets.Sources.Dissolvable;
using Assets.Sources.Utils;
using UnityEngine;

namespace Assets.Sources.PlayerScripts
{
    public class MedAid : DissolvableObject
    {
        [SerializeField, Min(0)] private float _healPower;

        public float HealPower => _healPower;

        private protected override void Start()
        {
            base.Start();
            DropDown();
            gameObject.layer = UserUtils.NormalLayer;
        }
    }
}