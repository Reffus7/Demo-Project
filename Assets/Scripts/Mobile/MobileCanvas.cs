using UnityEngine;
using UnityEngine.UI;

public class MobileCanvas : MonoBehaviour {
    [SerializeField] private FixedJoystick _joystick;
    [SerializeField] private Button _attackButton;
    [SerializeField] private Button _dodgeButton;
    [SerializeField] private Button _pauseButton;

    public FixedJoystick joystick => _joystick;
    public Button attackButton => _attackButton;
    public Button dodgeButton => _dodgeButton;
    public Button pauseButton => _pauseButton;
}
