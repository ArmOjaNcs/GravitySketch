using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private int _maxCapacity;

    private ObjectPool<Bullet> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Bullet>(_bulletPrefab, _maxCapacity, transform);
    }

    public void Shoot(Vector3 startPosition, Vector3 destination)
    {
        Bullet bullet = _pool.GetElement();
        bullet.Finished += OnFinished;
        bullet.gameObject.SetActive(true);
        bullet.Send(startPosition, destination);
    }

    private void OnFinished(Bullet bullet)
    {
        bullet.Finished -= OnFinished;
        bullet.gameObject.SetActive(false);
    }
}