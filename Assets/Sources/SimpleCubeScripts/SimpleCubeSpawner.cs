using Assets.Sources.Audio;
using Assets.Sources.Pause;
using Assets.Sources.Table;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.SimpleCubeScripts
{
    public class SimpleCubeSpawner : MonoBehaviour
    {
        [SerializeField] private SimpleCube _cubePrefab;
        [SerializeField] private float _spacing = 1.5f;
        [SerializeField] private float _yPosition = 0f;

        [HideInInspector]
        [SerializeField] private List<SimpleCube> _spawnedCubes = new List<SimpleCube>();

        private TemplateColorReference _materialReference;
        private List<SpawnArea> _spawnAreas = new List<SpawnArea>();
        private PauseHandler _pauseHandler;
        private AudioPlayerSpawner _audioPlayerSpawner;

        public int TotalCubes => _materialReference.GetTotalCount();

        public void PrepareQueue()
        {
            ClearAllCubes();

            if (ValidateReferences() == false)
                return;

            int totalColors = _materialReference.GetTotalCount();

            int totalSpawnCount = 0;

            foreach (SpawnArea area in _spawnAreas)
                totalSpawnCount += area.Count;

            if (totalSpawnCount > totalColors)
            {
                Debug.LogWarning($"Too many cubes to spawn ({totalSpawnCount}) vs available colors ({totalColors}). Clamping.");
            }

            int allowedSpawn = Mathf.Min(totalSpawnCount, totalColors);

            int spawnIndex = 0;

            foreach (SpawnArea area in _spawnAreas)
            {
                int cubesToSpawn = Mathf.Min(area.Count, allowedSpawn - spawnIndex);
                int gridSize = Mathf.CeilToInt(Mathf.Sqrt(cubesToSpawn));

                for (int i = 0; i < cubesToSpawn; i++)
                {
                    Vector3 pos = CalculatePosition(gridSize, i / gridSize, i % gridSize);
                    SimpleCube cube = Instantiate(_cubePrefab, area.transform);
                    cube.transform.localPosition = pos;
                    _spawnedCubes.Add(cube);
                    spawnIndex++;

                    if (spawnIndex >= allowedSpawn)
                        break;
                }

                if (spawnIndex >= allowedSpawn)
                    break;
            }

            Debug.Log($"Spawned {spawnIndex} cubes in editor. Colors will be assigned at runtime.");
        }

        public void Init(PauseHandler pauseHandler, AudioPlayerSpawner audioPlayerSpawner, 
            List<SpawnArea> spawnAreas, TemplateColorReference materialReference)
        {
            _pauseHandler = pauseHandler;
            _audioPlayerSpawner = audioPlayerSpawner;
            _materialReference = materialReference;
            _spawnAreas = spawnAreas;
            PrepareQueue();
            InitCubes();
            ApplyColors();
        }

        public void DropCubes()
        {
            foreach (SimpleCube simpleCube in _spawnedCubes)
                simpleCube.DropDown();
        }

        private void ApplyColors()
        {
            List<Color> colors = _materialReference.GetAllColors();
            List<Color> shuffledColors = ShuffleColors(colors);

            if (_spawnedCubes.Count > shuffledColors.Count)
            {
                Debug.LogWarning("More cubes than colors. Some cubes will get default color.");
            }

            for (int i = 0; i < _spawnedCubes.Count; i++)
            {
                if (i < shuffledColors.Count)
                    _spawnedCubes[i].SetColor(shuffledColors[i]);
                else
                    Debug.LogWarning($"Cube {i} has no color assigned");
            }
        }

        private void InitCubes()
        {
            foreach (SimpleCube cube in _spawnedCubes)
            {
                cube.Init(_pauseHandler);
                cube.SetAudioPlayerSpawner(_audioPlayerSpawner);
            }
        }

        private Vector3 CalculatePosition(int gridSize, int row, int col)
        {
            float x = (col - (gridSize - 1) * 0.5f) * _spacing;
            float z = (row - (gridSize - 1) * 0.5f) * _spacing;
            return new Vector3(x, _yPosition, z);
        }

        private List<Color> ShuffleColors(List<Color> colors)
        {
            for (int i = colors.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (colors[i], colors[j]) = (colors[j], colors[i]);
            }

            return colors;
        }

        private bool ValidateReferences()
        {
            if (_cubePrefab == null)
                Debug.LogError("Cube prefab missing!");
            if (_materialReference == null)
                Debug.LogError("Material reference missing!");
            if (_spawnAreas.Count == 0)
                Debug.LogError("No spawn areas!");

            return _cubePrefab && _materialReference && _spawnAreas.Count > 0;
        }

        private void ClearAllCubes()
        {
            foreach (SpawnArea area in _spawnAreas)
            {
                for (int i = area.transform.childCount - 1; i >= 0; i--)
                {
                    DestroyImmediate(area.transform.GetChild(i).gameObject);
                }
            }

            _spawnedCubes.Clear();
        }
    }
}