using System;
using UnityEngine;
using Zenject;

public class DesktopInputHandler : IInputHandler {

    private GameInput gameInput;

    public event Action onDodge;
    public event Action onAttack;
    public event Action onPause;
    public event Action<Vector2> onMove;

    [Inject]
    public void Construct(GameInput gameInput) {
        this.gameInput = gameInput;

        gameInput.Game.Move.performed += ctx => onMove?.Invoke(ctx.ReadValue<Vector2>());
        gameInput.Game.Move.canceled += ctx => onMove?.Invoke(Vector2.zero);

        gameInput.Game.Dodge.performed += ctx => onDodge?.Invoke();
        gameInput.Game.Attack.performed += ctx => onAttack?.Invoke();

        gameInput.Menu.Pause.performed += ctx => onPause?.Invoke();

        EnableInput();
    }

    public void EnableInput() {
        gameInput.Enable();
    }

    public void DisableGameInput() {
        gameInput.Game.Disable();

    }


}