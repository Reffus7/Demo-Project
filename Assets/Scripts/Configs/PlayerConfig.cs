using Project.Progress;
using UnityEngine;

namespace Project.Config {
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/PlayerConfig")]
    public class PlayerConfig : ScriptableObject {

        [Header("Player parameters")]
        [SerializeField] private PlayerProgressVar _moveSpeedProgress;
        [SerializeField] private PlayerProgressVar _rotationSpeedProgress;
        [SerializeField] private PlayerProgressVar _dodgeDistanceProgress;
        [SerializeField] private PlayerProgressVar _dodgeDurationProgress;
        [SerializeField] private PlayerProgressVar _dodgeInvincibilityProgress;
        [SerializeField] private PlayerProgressVar _dodgeCooldownProgress;
        [SerializeField] private PlayerProgressVar _attackRangeProgress;
        [SerializeField] private PlayerProgressVar _attackSpeedProgress;
        [SerializeField] private PlayerProgressVar _attackDamageProgress;
        [SerializeField] private PlayerProgressVar _maxHealthProgress;

        public PlayerProgressVar moveSpeedProgress=>_moveSpeedProgress;
        public PlayerProgressVar rotationSpeedProgress=>_rotationSpeedProgress;
        public PlayerProgressVar dodgeDistanceProgress=>_dodgeDistanceProgress;
        public PlayerProgressVar dodgeDurationProgress=>_dodgeDurationProgress;
        public PlayerProgressVar dodgeInvincibilityProgress=>_dodgeInvincibilityProgress;
        public PlayerProgressVar dodgeCooldownProgress=>_dodgeCooldownProgress;
        public PlayerProgressVar attackRangeProgress=>_attackRangeProgress;
        public PlayerProgressVar attackSpeedProgress=>_attackSpeedProgress;
        public PlayerProgressVar attackDamageProgress=> _attackDamageProgress;
        public PlayerProgressVar maxHealthProgress=>_maxHealthProgress;


    }
}