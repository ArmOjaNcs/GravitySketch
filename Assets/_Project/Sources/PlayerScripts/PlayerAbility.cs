using Assets.Sources.Pause;
using UnityEngine;

namespace Assets.Sources.PlayerScripts
{
    public abstract class PlayerAbility : PauseableObject
    {
        [SerializeField] private protected PlayerInput Input;
        [SerializeField, Min(0)] private protected float ActiveTime;
        [SerializeField, Min(0)] private protected float ReloadTime;
        [SerializeField, Min(0)] private protected float ReloadUpgradeDelta;
        
        public float CurrentReloadTime { get; protected set; }
        public float CurrentActiveTime { get; protected set; }

        public abstract void Upgrade();
    }
}