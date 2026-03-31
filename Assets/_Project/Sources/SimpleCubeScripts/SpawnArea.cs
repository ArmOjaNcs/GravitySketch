using UnityEngine;

namespace Assets.Sources.SimpleCubeScripts
{
    public class SpawnArea : MonoBehaviour
    {
        [SerializeField] private int _count;

        public int Count => _count;
    }
}