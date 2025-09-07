using Assets.Sources.Utils;
using UnityEngine;

namespace Assets.Sources.Dissolvable
{
    public class DissolvableObstacle : DissolvableObject
    {
        [SerializeField] private protected GameObject CollidersHolder = null;

        private protected override void Awake()
        {
            base.Awake();
            SetLayerRecursively(gameObject, UserUtils.ObstacleLayer);
            SetTagRecursively(gameObject, UserUtils.DissolvableObstacle);
        }

        public override void DropDown()
        {
            base.DropDown();

            SetLayerRecursively(gameObject, UserUtils.NormalLayer);
            SetTagRecursively(gameObject, UserUtils.Dropped);
        }

        public override void Dissolve(Transform hole)
        {
            base.Dissolve(hole);

            if (CollidersHolder != null)
                CollidersHolder.SetActive(false);
        }

        private void SetLayerRecursively(GameObject obj, int newLayer)
        {
            obj.layer = newLayer;

            foreach (Transform child in obj.transform)
                SetLayerRecursively(child.gameObject, newLayer);
        }

        private void SetTagRecursively(GameObject obj, string newTag)
        {
            obj.tag = newTag;

            foreach (Transform child in obj.transform)
                SetTagRecursively(child.gameObject, newTag);
        }
    }
}