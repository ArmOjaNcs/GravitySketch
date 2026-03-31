using DG.Tweening;
using UnityEngine;

public class DOTweenBootstrap : MonoBehaviour
{
    private void Awake()
    {
        DOTween.SetTweensCapacity(500, 100);
        DOTween.useSafeMode = false;
    }
}