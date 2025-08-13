using Assets.Sources.Dissolvable;
using Assets.Sources.Utils;
using DG.Tweening;
using System;
using UnityEngine;

namespace Assets.Sources.Level
{
    public class LevelExit : DissolvableObject
    {
        public event Action Exit;

        public bool IsDowned { get; private set; }

        public override void DropDown()
        {
            base.DropDown();
            gameObject.layer = UserUtils.NormalLayer;
            IsDowned = true;
        }

        public override void Dissolve(Transform hole)
        {
            Debug.Log($"Is Animation null {DissolveAnimation == null}");
            Debug.Log($"Animation duration {DissolveAnimation.Duration()}");
            base.Dissolve(hole);
            Exit?.Invoke();
        }

        private protected override void OnRoutineStart()
        {
            base.OnRoutineStart();
            Debug.Log($"Elapsed Time {ElapsedTime} On Start");
        }

        private protected override void OnRoutineEnd()
        {
            base.OnRoutineEnd();
            Debug.Log($"Elapsed Time {ElapsedTime} On End");
        }
    }
}