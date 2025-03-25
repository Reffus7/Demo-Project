using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Mobile {

    public class MobileCanvas : MonoBehaviour {
        [SerializeField] private FloatingJoystick _joystick;
        [SerializeField] private Button _attackButton;
        [SerializeField] private Button _dodgeButton;
        [SerializeField] private Button _pauseButton;

        public FloatingJoystick joystick => _joystick;
        public Button attackButton => _attackButton;
        public Button dodgeButton => _dodgeButton;
        public Button pauseButton => _pauseButton;
    }
}