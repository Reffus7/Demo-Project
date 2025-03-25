using System;
using UnityEngine;

namespace Project.Input {
    public interface IInputHandler {
        public event Action onDodge;
        public event Action onAttack;
        public event Action onPause;
        public event Action<Vector2> onMove;

        public void DisableInput();

        public void EnableInput();
    }
}