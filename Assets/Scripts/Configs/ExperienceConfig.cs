using Project.Progress;
using UnityEngine;

namespace Project.Config {
    [CreateAssetMenu(fileName = "ExperienceConfig", menuName = "Configs/ExperienceConfig")]
    public class ExperienceConfig : ScriptableObject {
        [SerializeField] private PlayerProgressVar _playerLevelExperienceProgress;
        [SerializeField] private EnemyProgressVar _enemyExperienceProgress;

        public PlayerProgressVar playerLevelExperienceProgress => _playerLevelExperienceProgress;
        public EnemyProgressVar enemyExperienceProgress => _enemyExperienceProgress;

    }
}