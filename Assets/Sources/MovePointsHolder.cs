using System.Collections.Generic;
using UnityEngine;

public class MovePointsHolder : MonoBehaviour
{
    [SerializeField] private List<Transform> _movePoints;

    public Transform GetMovePoint()
    {
        return _movePoints[Random.Range(0, _movePoints.Count)];
    }  
}