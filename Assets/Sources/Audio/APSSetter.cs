using Assets.Sources.Dissolvable;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.Audio
{
    public class APSSetter : MonoBehaviour
    {
        [SerializeField] private AudioPlayerSpawner _audioPlayerSpawner;
        [SerializeField] private List<DissolvableObject> _objects;

        private void Start()
        {
            foreach (DissolvableObject obj in _objects)
                obj.SetAudioPlayerSpawner(_audioPlayerSpawner);
        }
    }
}