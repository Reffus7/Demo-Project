using System;
using UnityEngine;

public interface IInputHandler {
    public event Action onDodge;
    public event Action onAttack;
    public event Action onPause;
    public event Action<Vector2> onMove;


    public void DisableGameInput();
}