using Project.Progress;
using UnityEngine;

namespace Project.Config {
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Configs/EnemyConfig")]
    public class EnemyConfig : ScriptableObject {

        [SerializeField] private float _attackRange = 10f;
        [SerializeField] private float _fleeRange = 5f;

        public float attackRange => _attackRange;
        public float fleeRange => _fleeRange;

        [Header("Difficulty parameters")]

        [SerializeField] private EnemyProgressVar _healthMaxProgress;
        [SerializeField] private EnemyProgressVar _projectileSpeedProgress;
        [SerializeField] private EnemyProgressVar _reloadTimeProgress;
        [SerializeField] private EnemyProgressVar _moveSpeedProgress;
        [SerializeField] private EnemyProgressVar _rotateSpeedProgress;
        [SerializeField] private EnemyProgressVar _damageProgress;
        [SerializeField] private EnemyProgressVar _attackSpeedProgress;

        public EnemyProgressVar healthMaxProgress => _healthMaxProgress;
        public EnemyProgressVar projectileSpeedProgress => _projectileSpeedProgress;
        public EnemyProgressVar reloadTimeProgress => _reloadTimeProgress;
        public EnemyProgressVar moveSpeedProgress => _moveSpeedProgress;
        public EnemyProgressVar rotateSpeedProgress => _rotateSpeedProgress;
        public EnemyProgressVar damageProgress => _damageProgress;
        public EnemyProgressVar attackSpeedProgress => _attackSpeedProgress;


    }
}