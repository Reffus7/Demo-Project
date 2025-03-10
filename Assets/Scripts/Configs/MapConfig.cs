using Project.Map;
using Project.Progress;
using UnityEngine;

namespace Project.Config {
    [CreateAssetMenu(fileName = "MapConfig", menuName = "Configs/MapConfig")]
    public class MapConfig : ScriptableObject {
        [SerializeField] private Portal _portalPrefab;
        [SerializeField] private int _portalsInRoom = 4;

        public Portal portalPrefab => _portalPrefab;
        public int portalsInRoom => _portalsInRoom;

        [Header("Difficulty parameters")]
        [SerializeField] private EnemyProgressVar _roomCountProgress;

        public EnemyProgressVar roomCountProgress => _roomCountProgress;

    }
}