using System;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class ColorizedCube : MonoBehaviour
{
    private Vector3 _rotateDirection;
    private float _speed;
    private MeshRenderer _meshRenderer;
    private IReadonlyTemplateCube _target;
    private bool _isCanMove;
    private bool _isInitiated;
    private Transform _transform;

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

        _isInitiated = true;
        _transform = transform;
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void SetStartSettings(ColorizedCubeData colorizedCubeData)
    {
        if (_isInitiated == false)
            Init();

        _transform.position = colorizedCubeData.StartPosition;
        _target = colorizedCubeData.TemplateCube;
        _meshRenderer.material = colorizedCubeData.Material;
        _speed = colorizedCubeData.Speed;
        _rotateDirection = colorizedCubeData.RotateDirection;
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
        _transform.position = Vector3.MoveTowards(_transform.position, _target.Position, _speed * Time.deltaTime);

        if (_transform.position == _target.Position)
        {
            _isCanMove = false;
            _target.SetColored(_meshRenderer.material);
            Finished?.Invoke(this);
        }

        Rotate();
    }

    private void Rotate() => _transform.Rotate(_rotateDirection * _speed * Time.deltaTime);
}