using Assets.Sources.Dissolvable;
using Assets.Sources.EnemyScripts;
using Assets.Sources.SimpleCubeScripts;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.Level
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private EnemyFactory _enemyFactory;
        [SerializeField] private List<DissolvableObstacle> _obstacles;
        [SerializeField] private SimpleCubeSpawner _simpleCubeSpawner;
    }
}