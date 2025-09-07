using System;
using UnityEngine;

namespace Assets.Sources.PlayerScripts
{
    public class PlayerPointMover : PointMover
    {
        public event Action<Vector3> PositionChanged;

        private void LateUpdate()
        {
            if (IsPaused || IsInitialized == false)
                return;

            PositionChanged?.Invoke(Transform.position);
        }
    }
}