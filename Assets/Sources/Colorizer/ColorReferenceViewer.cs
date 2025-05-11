using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorReferenceViewer : MonoBehaviour
{
    [SerializeField] private Template _template;
    [SerializeField] private TemplateMaterialReference _materialReference;
    [SerializeField] private Button _showButton;

    private IReadOnlyList<IReadonlyCube> _templateCubes;
    private Coroutine _showingRoutine;
    private WaitForSeconds _waitForShowing;

    public event Action<bool> ReferenceShowed;

    private void Awake()
    {
        _templateCubes = _template.TemplateCubes;
        _waitForShowing = new WaitForSeconds(UserUtils.TimeForShow);
    }

    private void OnEnable()
    {
        _showButton.onClick.AddListener(ShowReference);
    }

    private void OnDisable()
    {
        _showButton.onClick.RemoveListener(ShowReference);
    }

    private void ShowReference()
    {
        if (_showingRoutine != null)
            return;

        Debug.Log("clicked");
        _showingRoutine = StartCoroutine(ShowRoutine());
    }

    private IEnumerator ShowRoutine()
    {
        ReferenceShowed?.Invoke(true);
        _materialReference.HightLightAllCubes(_templateCubes);
        yield return _waitForShowing;

        foreach (IReadonlyCube cube in _templateCubes)
            cube.StopHighlight();

        ReferenceShowed?.Invoke(false);
        _showingRoutine = null;
    }
}