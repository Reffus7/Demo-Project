using Project.UI.Mobile;
using System;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Project.Input {

    public class MobileInputHandler : IInputHandler {
        public event Action onDodge;
        public event Action onAttack;
        public event Action onPause;
        public event Action<Vector2> onMove;

        private MobileCanvas mobileCanvas;

        [Inject]
        public void Construct(MobileCanvas mobileCanvas) {
            this.mobileCanvas = mobileCanvas;

            EnableInput();

        }

        public void EnableInput() {
            mobileCanvas.pauseButton.onClick.AddListener(OnPause);

            mobileCanvas.joystick.onMove += OnMove;

            mobileCanvas.dodgeButton.AddComponent<EventsButton>().onHolding += OnDodge;
            mobileCanvas.attackButton.AddComponent<EventsButton>().onHolding += OnAttack;
        }

        public void DisableInput() {
            mobileCanvas.pauseButton.onClick.RemoveListener(OnPause);

            mobileCanvas.joystick.onMove -= OnMove;

            mobileCanvas.dodgeButton.GetComponent<EventsButton>().onHolding -= OnDodge;
            mobileCanvas.attackButton.GetComponent<EventsButton>().onHolding -= OnAttack;
        }

        private void OnPause() {
            onPause?.Invoke();
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
}