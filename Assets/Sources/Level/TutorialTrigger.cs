using System;
using UnityEngine;

namespace Assets.Sources.Level
{
    [RequireComponent(typeof(BoxCollider))]
    public class TutorialTrigger : MonoBehaviour
    {
        [SerializeField] private TutorialTriggerType _type;

        public event Action<TutorialTriggerType> PlayerInZone;

        private void OnTriggerEnter(Collider other)
        {
            PlayerInZone?.Invoke(_type);
            gameObject.SetActive(false);
        }
    }
}