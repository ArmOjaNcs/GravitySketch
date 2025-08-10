using Assets.Sources.Dissolvable;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.Audio
{
    public class APSSetter : MonoBehaviour
    {
        [SerializeField] private List<DissolvableObject> _objects;

        public void SetAPS(AudioPlayerSpawner audioPlayerSpawner)
        {
            foreach (DissolvableObject obj in _objects)
                obj.SetAudioPlayerSpawner(audioPlayerSpawner);
        }
    }
}