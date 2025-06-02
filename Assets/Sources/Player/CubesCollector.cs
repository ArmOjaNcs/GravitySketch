using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CubesCollector : MonoBehaviour
{
    [SerializeField] private TakeOverLimit _takeOverLimit;
    [SerializeField] private Shield _shield;
    [SerializeField] private Transform _bottom;

    private CubesHolder _holder = new CubesHolder();
    private IReadOnlyList<SimpleCube> _toThrowUp;

    public event Action<int> CubesCountChanged;

    public bool IsDefended => _shield.IsDefended;

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

    public void TakeDamage()
    {
        _toThrowUp = _holder.GetCubes(10);
        CubesCountChanged?.Invoke(_holder.Count);

        if (_toThrowUp == null)
            return;

        if(_toThrowUp.Count > 0)
        {
            foreach (SimpleCube cube in _toThrowUp)
            {
                cube.gameObject.SetActive(true);
                cube.ThrowOut(transform.position);
            }
        }
    }

    private void OnCubeAbsorbed(SimpleCube simpleCube)
    {
        if (_holder.Cubes.Contains(simpleCube))
            return;

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