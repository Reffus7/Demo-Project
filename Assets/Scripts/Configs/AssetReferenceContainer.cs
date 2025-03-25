using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Project.Config {
    [CreateAssetMenu()]
    public class AssetReferenceContainer : ScriptableObject {
        [Header("Player")]
        [SerializeField] private AssetReference _player;

        [Header("UI")]
        [SerializeField] private AssetReference _playerUI;
        [SerializeField] private AssetReference _mobileCanvas;
        [SerializeField] private AssetReference _adsMenuCanvas;

        [Header("Configs")]
        [SerializeField] private AssetReference _mapConfig;
        [SerializeField] private AssetReference _roomConfig;
        [SerializeField] private AssetReference _enemyConfig;
        [SerializeField] private AssetReference _experienceConfig;
        [SerializeField] private AssetReference _playerConfig;

        [Header("Scenes")]
        [SerializeField] private AssetReference _mainMenu;
        [SerializeField] private AssetReference _gameScene;

        [Header("Prefabs")]
        [SerializeField] private AssetReference _projectilePrefab;


        public AssetReference player => _player;
        public AssetReference playerUI => _playerUI;
        public AssetReference mobileCanvas => _mobileCanvas;
        public AssetReference adsMenuCanvas => _adsMenuCanvas;
        public AssetReference mapConfig => _mapConfig;
        public AssetReference roomConfig => _roomConfig;
        public AssetReference enemyConfig => _enemyConfig;
        public AssetReference experienceConfig => _experienceConfig;
        public AssetReference playerConfig => _playerConfig;
        public AssetReference mainMenuScene => _mainMenu;
        public AssetReference gameScene => _gameScene;
        public AssetReference projectilePrefab => _projectilePrefab;


    }
}