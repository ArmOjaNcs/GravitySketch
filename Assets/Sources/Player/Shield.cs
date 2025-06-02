using UnityEngine;

[RequireComponent (typeof(MeshRenderer))]
public class Shield : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private float _defendTime = 2f;
    [SerializeField] private float _reloadTime;

    private MeshRenderer _meshRenderer;
    private float _currentReloadTime;
    private float _totalTime;
    private bool _isDefended;
    private bool _isDefendApplied;

    public bool IsDefended => _isDefended;

    private void Awake()
    {
        _totalTime = _reloadTime + _defendTime;
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.enabled = false;
    }

    private void OnEnable()
    {
        _playerInput.Defended += OnDefended;
    }

    private void OnDisable()
    {
        _playerInput.Defended -= OnDefended;
    }

    private void Update()
    {
        if (_isDefendApplied)
            PlayCycle();
    }

    private void OnDefended()
    {
        if (_isDefendApplied)
            return;

        _isDefendApplied = true;
        _isDefended = true;
        _meshRenderer.enabled = true;
    }

    private void PlayCycle()
    {
        _currentReloadTime += Time.deltaTime;

        if (_currentReloadTime > _defendTime)
        {
            _isDefended = false;
            _meshRenderer.enabled = false;
        }

        if (_currentReloadTime > _totalTime)
        {
            _currentReloadTime = 0;
            _isDefendApplied = false;
        }
    }
}