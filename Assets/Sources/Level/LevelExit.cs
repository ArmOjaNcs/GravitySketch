using System;
using UnityEngine;
using Assets.Sources.Utils;
using Assets.Sources.Dissolvable;

namespace Assets.Sources.Level
{
    public class LevelExit : DissolvableObject
    {
        public event Action Exit;

        public bool IsDowned { get; private set; }

        public override void DropDown()
        {
            gameObject.SetActive(true);
            base.DropDown();
            gameObject.layer = UserUtils.NormalLayer;
            IsDowned = true;
        }

        public override void Dissolve(Transform hole)
        {
            Exit?.Invoke();
            base.Dissolve(hole);
        }
    }
}