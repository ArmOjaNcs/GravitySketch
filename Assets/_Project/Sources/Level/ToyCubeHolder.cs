using Dissolvable;
using UnityEngine;

namespace Level
{
    public class ToyCubeHolder : MonoBehaviour
    {
        [SerializeField] private DissolvableObstacle[] _cubes;
        [SerializeField] private Vector3 _position;

        public Vector3 Position => _position;

        public DissolvableObstacle[] ToyCubes => _cubes;
    }
}