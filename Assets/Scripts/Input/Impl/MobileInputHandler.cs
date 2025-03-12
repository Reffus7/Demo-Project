using System;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class MobileInputHandler : IInputHandler {
    public event Action onDodge;
    public event Action onAttack;
    public event Action onPause;
    public event Action<Vector2> onMove;

    private MobileCanvas mobileCanvas;

    [Inject]
    public void Construct(MobileCanvas mobileCanvas) {
        this.mobileCanvas = mobileCanvas;

        mobileCanvas.pauseButton.onClick.AddListener(() => onPause?.Invoke());

        mobileCanvas.joystick.onMove += OnMove;

        mobileCanvas.dodgeButton.AddComponent<EventsButton>().onHolding += OnDodge;
        mobileCanvas.attackButton.AddComponent<EventsButton>().onHolding += OnAttack;

    }
    public void DisableGameInput() {
        mobileCanvas.joystick.onMove -= OnMove;

        mobileCanvas.dodgeButton.GetComponent<EventsButton>().onHolding -= OnDodge;
        mobileCanvas.attackButton.GetComponent<EventsButton>().onHolding -= OnAttack;


    }

    private void OnDodge() {
        onDodge?.Invoke();
    }

    private void OnAttack() {
        onAttack?.Invoke();
    }

    private void OnMove(Vector2 direction) {
        onMove?.Invoke(direction);
    }

}