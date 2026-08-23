using System;
using Dissolvable;
using Utils;
using UnityEngine;

namespace Level
{
    public class LevelExit : DissolvableObstacle
    {
        public event Action Exit;

        public override void DropDown()
        {
            base.DropDown();
            GameObject localMesh = Resources.Load<GameObject>("Exit_" + Translator.CurrentLang);
            localMesh = Instantiate(localMesh);
            localMesh.transform.SetParent(transform, false);
            localMesh.transform.localPosition = Vector3.zero;
        }

        public override void Dissolve(Transform hole)
        {
            base.Dissolve(hole);
            Exit?.Invoke();
        }
    }
}