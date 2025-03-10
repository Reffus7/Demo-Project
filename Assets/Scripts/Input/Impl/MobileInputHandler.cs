
using System;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class MobileInputHandler : IInputHandler {
    public event Action onDodge;
    public event Action onAttack;
    public event Action onPause;
    public event Action<Vector2> onMove;

    [Inject]
    public void Construct(MobileCanvas mobileCanvas) {
        mobileCanvas.joystick.onMove += direction => onMove?.Invoke(direction);

        mobileCanvas.dodgeButton.AddComponent<EventsButton>().onHolding += () => onDodge?.Invoke();
        mobileCanvas.attackButton.AddComponent<EventsButton>().onHolding += () => onAttack?.Invoke();

        mobileCanvas.pauseButton.onClick.AddListener(() => onPause?.Invoke());
    }

    public void DisableGameInput() {
        return;
    }
}