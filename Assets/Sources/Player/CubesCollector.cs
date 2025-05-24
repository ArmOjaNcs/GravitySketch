using System;
using UnityEngine;

public class CubesCollector : MonoBehaviour
{
    [SerializeField] private TakeOverLimit _takeOverLimit;
    [SerializeField] private Transform _bottom;

    private CubesHolder _holder = new CubesHolder();

    public event Action<int> CubesCountChanged;

    private void OnEnable()
    {
        _takeOverLimit.CubeAbsorbed += OnCubeAbsorbed;
    }

    private void OnDisable()
    {
        _takeOverLimit.CubeAbsorbed -= OnCubeAbsorbed;
    }

    private void Start()
    {
        CubesCountChanged?.Invoke(_holder.Count);
    }

    private void OnCubeAbsorbed(SimpleCube simpleCube)
    {
        _holder.AddCube(simpleCube);
        CubesCountChanged?.Invoke(_holder.Count);
        simpleCube.Dissolved += OnDissolved;
        simpleCube.Dissolve(_bottom);
    }

    private void OnDissolved(SimpleCube simpleCube)
    {
        simpleCube.Dissolved -= OnDissolved;
        simpleCube.gameObject.SetActive(false);
    }
}