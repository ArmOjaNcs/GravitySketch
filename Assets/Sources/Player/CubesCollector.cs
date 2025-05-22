using System;
using UnityEngine;

public class CubesCollector : MonoBehaviour
{
    [SerializeField] private Catcher _catcher;
    [SerializeField] private Transform _bottom;

    private CubesHolder _holder = new CubesHolder();

    public event Action<int> CubesCountChanged;

    private void OnEnable()
    {
        _catcher.CubeCatched += OnCubeCatched;
    }

    private void OnDisable()
    {
        _catcher.CubeCatched -= OnCubeCatched;
    }

    private void Start()
    {
        CubesCountChanged?.Invoke(_holder.Count);
    }

    private void OnCubeCatched(SimpleCube simpleCube)
    {
        _holder.AddCube(simpleCube);
        CubesCountChanged?.Invoke(_holder.Count);
        simpleCube.Dissolved += OnDissolved;
        simpleCube.Dissolve(_bottom.position);
    }

    private void OnDissolved(SimpleCube simpleCube)
    {
        simpleCube.Dissolved -= OnDissolved;
        simpleCube.gameObject.SetActive(false);
    }
}