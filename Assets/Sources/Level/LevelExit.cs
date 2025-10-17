using Assets.Sources.Dissolvable;
using Assets.Sources.Utils;
using System;
using UnityEngine;

namespace Assets.Sources.Level
{
    public class LevelExit : DissolvableObstacle
    {
        public event Action Exit;

        public override void Dissolve(Transform hole)
        {
            base.Dissolve(hole);
            Exit?.Invoke();
        }
    }
}