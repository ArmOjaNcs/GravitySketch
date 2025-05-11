using System;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class ColorizedCube : MonoBehaviour
{
    private float _speed;
    private MeshRenderer _meshRenderer;
    private TemplateCube _target;
    private bool _isCanMove;
    private bool _isInitiated; 

    public event Action<ColorizedCube> Finished;

    private void Update()
    {
        if (_isCanMove && isActiveAndEnabled)
            MoveToTarget();
    }

    public void Init()
    {
        gameObject.SetActive(true);

        if (_isInitiated)
            return;

        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void SetStartSettings(Vector3 startPosition, TemplateCube templateCube, Material material, float speed)
    {
        SetStartPosition(startPosition);
        SetTarget(templateCube);
        _meshRenderer.material = material;
        SetSpeed(speed);
    }

    public void StartMove() => _isCanMove = true;

    public int GetTargetIndex()
    {
        if (_target == null)
            throw new ArgumentNullException(nameof(_target));

        return _target.Index;
    }

    private void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _speed * Time.deltaTime);

        if(transform.position == _target.transform.position)
        {
            StopMove();
            _target.SetColored(_meshRenderer.material);
            Finished?.Invoke(this);
        }
    }

    private void SetSpeed(float speed)
    {
        if (speed <= 0)
            throw new ArgumentOutOfRangeException("speed can not be negative value");

        _speed = speed;
    }

    private void SetStartPosition(Vector3 position) => transform.position = position;
    private void StopMove() => _isCanMove = false;
    private void SetTarget(TemplateCube templateCube) => _target = templateCube;
}