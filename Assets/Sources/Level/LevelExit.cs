using Assets.Sources.Dissolvable;
using Assets.Sources.Utils;
using System;
using UnityEngine;

namespace Assets.Sources.Level
{
    public class LevelExit : DissolvableObstacle
    {
        public event Action Exit;

        public override void DropDown()
        {
            base.DropDown();
            GameObject localMesh = Resources.Load<GameObject>("Exit_"+Translator.CurrentLang);
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