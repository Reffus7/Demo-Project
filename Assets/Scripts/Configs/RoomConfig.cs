using Project.Enemy;
using Project.Progress;
using UnityEngine;

namespace Project.Config {

    [CreateAssetMenu(fileName = "RoomConfig", menuName = "Configs/RoomConfig")]
    public class RoomConfig : ScriptableObject {
        [SerializeField] private Transform _wallPrefab;
        [SerializeField] private Transform _floorPrefab;
        [SerializeField] private Light _lightPrefab;
        [SerializeField] private GameObject[] _objectPrefabs;
        [SerializeField] private EnemyBase[] _enemyPrefabs;

        [SerializeField] private float _wallHeight = 2;
        [SerializeField] private float _wallWidth = 0.1f;
        [SerializeField] private float _minDistanceBetweenObjects = 1;
        [SerializeField] private float _offsetFromWalls = 2.5f;

        [SerializeField] private Vector2Int _minMaxSize;
        [SerializeField] private Vector2Int _minMaxObjects;

        [Header("Difficulty parameters")]
        [SerializeField] private EnemyProgressVar _minEnemiesProgress;
        [SerializeField] private EnemyProgressVar _maxEnemiesProgress;

        public EnemyProgressVar minEnemiesProgress => _minEnemiesProgress;
        public EnemyProgressVar maxEnemiesProgress => _maxEnemiesProgress;


        public Transform wallPrefab => _wallPrefab;
        public Transform floorPrefab => _floorPrefab;
        public Light lightPrefab => _lightPrefab;
        public GameObject[] objectPrefabs => _objectPrefabs;
        public EnemyBase[] enemyPrefabs => _enemyPrefabs;

        public float wallHeight => _wallHeight;
        public float wallWidth => _wallWidth;
        public float minDistanceBetweenObjects => _minDistanceBetweenObjects;
        public float offsetFromWalls => _offsetFromWalls;

        public Vector2Int minMaxSize => _minMaxSize;
        public Vector2Int minMaxObjects => _minMaxObjects;

    }
}