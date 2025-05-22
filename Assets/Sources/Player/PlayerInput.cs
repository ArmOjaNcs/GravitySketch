using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public event Action<Vector2> DirectionChanged;
    public event Action<bool> Boosted;

    private float HorizontalInput => Input.GetAxis(UserUtils.Horizontal);
    private float VerticalInput => Input.GetAxis(UserUtils.Vertical);
    private bool IsBoosted => Input.GetKeyDown(KeyCode.Mouse1);

    private void Update()
    {
        DirectionChanged?.Invoke(new Vector2(HorizontalInput, VerticalInput));
        Boosted?.Invoke(IsBoosted);
    }
}