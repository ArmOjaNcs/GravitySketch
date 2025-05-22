using UnityEngine;

[RequireComponent (typeof(BoxCollider))]
public class Pusher : MonoBehaviour
{
    [SerializeField] private float _pushForce;
    [SerializeField] private float _blinkRate;
    
    private Transform _transform;
    private BoxCollider _boxCollider;
    private float _currentBlinkTime;

    private void Awake()
    {
        _transform = transform;
        _boxCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
       // Blink();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Mover mover))
            mover.ApplyExternalForce((mover.transform.position - _transform.position).normalized * _pushForce * Time.deltaTime);
    }

    private void Blink()
    {
        _currentBlinkTime += Time.deltaTime;

        if(_currentBlinkTime > _blinkRate)
        {
            _currentBlinkTime = 0;
            _boxCollider.enabled = !_boxCollider.enabled;
        }
    }
}