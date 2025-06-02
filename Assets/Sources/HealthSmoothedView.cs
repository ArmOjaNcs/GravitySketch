using System.Collections;
using UnityEngine;

public abstract class HealthSmoothedView : MonoBehaviour
{
    [SerializeField] private protected Health Health;
    [SerializeField] private protected float CurrentSmoothDuration;

    private protected float SmoothDuration;
    private protected Coroutine Coroutine;

    private void OnEnable()
    {
        Health.HealthUpdate += OnHealthUpdate;
    }

    private void OnDisable()
    {
        Health.HealthUpdate -= OnHealthUpdate;

        if (Coroutine != null)
        {
            StopCoroutine(Coroutine);
            Coroutine = null;
        }
    }

    private protected abstract IEnumerator UpdateView();

    private void OnHealthUpdate()
    {
        if (Coroutine != null)
            StopCoroutine(Coroutine);

        Coroutine = StartCoroutine(UpdateView());
    }
}