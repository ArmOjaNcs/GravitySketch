using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.Utils
{
    [RequireComponent(typeof(BoxCollider))]
    public class ObjectsSwitcher : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _objects;

        private void Awake()
        {
            foreach (GameObject obj in _objects)
                obj.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                foreach (GameObject obj in _objects)
                    obj.SetActive(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                foreach (GameObject obj in _objects)
                    obj.SetActive(false);
            }
        }
    }
}