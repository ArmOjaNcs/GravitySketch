using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent (typeof(MeshRenderer))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _speed;
    [SerializeField] private float _lifeTime;
    [SerializeField] private float _radius;
    [SerializeField] private float _force;
    [SerializeField] private ParticleSystem _effect;

    private Rigidbody _rigidbody;
    private Rigidbody _playerRigidbody;
    private CubesCollector _player;
    private Transform _transform;
    private MeshRenderer _meshRenderer;
    private Vector3 _destination;
    private Vector3 _direction;
    private bool _isSended;
    private float _currentLifeTime;
    private float _explosionTime;
    private float _currentExplosionTime;
    private bool _isExploding;

    public event Action<Bullet> Finished;

    public float Damage => _damage;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;
        _transform = transform;
        _explosionTime = _effect.main.duration;
        _effect.Stop();
    }

    private void OnEnable()
    {
        _effect.Stop();
        _isSended = false;
        _meshRenderer.enabled = true;
    }

    private void OnDisable()
    {
        _isExploding = false;
        _effect.Stop();
    }

    private void Update()
    {
        if (_isExploding)
        {
            _currentExplosionTime += Time.deltaTime;

            if (_currentExplosionTime > _explosionTime)
            {
                _currentExplosionTime = 0;
                Finished?.Invoke(this);
            }
        }

        if (_isSended == false)
            return;

        if (_isExploding == false)
            Live();
    }

    private void FixedUpdate()
    {
        if (_isSended == false)
            return;

        Vector3 move = _direction * _speed * Time.fixedDeltaTime;
        _rigidbody.MovePosition(_rigidbody.position + move);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
            EndLife();
    }

    public void Send(Vector3 startPosition, Vector3 destination)
    {
        _transform.position = startPosition;
        _destination = destination;

        _direction = (_destination - _transform.position).normalized;

        _isSended = true;
        _currentLifeTime = 0;
    }

    private void Live()
    {
        _currentLifeTime += Time.deltaTime;

        if (_currentLifeTime > _lifeTime)
            EndLife();
    }

    private void EndLife()
    {
        _currentLifeTime = 0;
        _isSended = false;
        Exploid();
    }

    private void Exploid()
    {
        _isExploding = true;
        _meshRenderer.enabled = false;
        _effect.Play();
        Collider[] colliders = Physics.OverlapSphere(_transform.position, _radius);
       
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                if (_player == null)
                    _player = collider.GetComponentInChildren<CubesCollector>();

                if (_playerRigidbody == null)
                    _playerRigidbody = collider.GetComponent<Rigidbody>();

                if(_player.IsDefended == false)
                {
                    _player.TakeDamage();
                    Vector3 forceVector = (_player.transform.position - _transform.position).normalized;
                    forceVector.y = 0;
                    _playerRigidbody.AddForceAtPosition(forceVector * 100, _transform.position, ForceMode.Impulse);
                }
                
                break;
            }
        }
    }
}