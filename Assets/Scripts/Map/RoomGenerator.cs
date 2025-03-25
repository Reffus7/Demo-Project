using Cysharp.Threading.Tasks;
using Project.Config;
using Project.Factory;
using Project.Progress;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Project.Map {

    public class RoomGenerator : IInitializable {
        private Transform roomParent;

        private Vector3 roomPosition;
        List<Light> lights = new();
        List<Vector3> enemyPositions = new();

        private Vector2 size;
        private Vector2 sizeHalf => size / 2;

        private HashSet<Vector3> occupiedPositions;

        private Vector2Int minMaxEnemies => new Vector2Int(
            (int)minEnemiesProgress.Value,
            (int)maxEnemiesProgress.Value
            );

        private RoomConfig roomConfig;

        private EnemyProgressVar minEnemiesProgress;
        private EnemyProgressVar maxEnemiesProgress;

        private EnemyProgressVarFactory enemyProgressVarFactory;
        private AssetReferenceContainer assetReferenceContainer;
        private AssetProvider assetProvider;

        [Inject]
        public void Construct(
            EnemyProgressVarFactory enemyProgressVarFactory,
            AssetReferenceContainer assetReferenceContainer,
            AssetProvider assetProvider

        ) {


            this.enemyProgressVarFactory = enemyProgressVarFactory;
            this.assetProvider = assetProvider;
            this.assetReferenceContainer = assetReferenceContainer;
        }

        public void Initialize() {
            InitAsync().Forget();

        }

        private async UniTaskVoid InitAsync() {
            roomConfig = await assetProvider.LoadAssetAsync<RoomConfig>(assetReferenceContainer.roomConfig);

            minEnemiesProgress = enemyProgressVarFactory.Create(roomConfig.minEnemiesProgress);
            maxEnemiesProgress = enemyProgressVarFactory.Create(roomConfig.maxEnemiesProgress);
        }

        public RoomInfo GenerateRoom(Vector3 position) {
            roomPosition = position;

            size = new Vector2(
                Random.Range(roomConfig.minMaxSize.x, roomConfig.minMaxSize.y),
                Random.Range(roomConfig.minMaxSize.x, roomConfig.minMaxSize.y)
                );

            CreateRoomParent();

            CreateFloor();

            CreateWall(new Vector3(sizeHalf.x, 0, 0), Vector3.up * 90, size.y);
            CreateWall(new Vector3(-sizeHalf.x, 0, 0), Vector3.up * 90, size.y);
            CreateWall(new Vector3(0, 0, sizeHalf.y), Vector3.zero, size.x);
            CreateWall(new Vector3(0, 0, -sizeHalf.y), Vector3.zero, size.x);

            lights = new();
#if UNITY_ANDROID
            CreateLight(new Vector3(0, 2, 0));
#else
            CreateLight(new Vector3(sizeHalf.x - 1, 2, sizeHalf.y - 1));
            CreateLight(new Vector3(-sizeHalf.x + 1, 2, sizeHalf.y - 1));
            CreateLight(new Vector3(sizeHalf.x - 1, 2, -sizeHalf.y + 1));
            CreateLight(new Vector3(-sizeHalf.x + 1, 2, -sizeHalf.y + 1));
#endif

            occupiedPositions = new HashSet<Vector3>();

            enemyPositions = new();
            SetEnemyPlaceInRoom(Random.Range(minMaxEnemies.x, minMaxEnemies.y + 1));

            SpawnObjectsInRoom(roomConfig.objectPrefabs, Random.Range(roomConfig.minMaxObjects.x, roomConfig.minMaxObjects.y + 1));

            return new RoomInfo { roomParent = roomParent, roomPosition = roomPosition, size = size, lights = lights, enemyPositions = enemyPositions };
        }

        private void CreateRoomParent() {
            roomParent = new GameObject("Room").transform;
            roomParent.position = roomPosition;
        }

        private void CreateFloor() {
            float planeScale = 10;
            Transform floor = Object.Instantiate(roomConfig.floorPrefab, roomPosition, Quaternion.identity, roomParent);
            floor.localScale = new Vector3(size.x / planeScale, 1, size.y / planeScale);
        }

        private void SetEnemyPlaceInRoom(int amount) {
            for (int i = 0; i < amount; i++) {
                Vector3 objPos = GetValidSpawnPosition();
                if (objPos == Vector3.zero) return;
                occupiedPositions.Add(objPos);

                enemyPositions.Add(objPos);
            }
        }

        private void SpawnObjectsInRoom(GameObject[] prefabs, int amount) {
            for (int i = 0; i < amount; i++) {
                Vector3 objPos = GetValidSpawnPosition();
                if (objPos == Vector3.zero) return;
                occupiedPositions.Add(objPos);

                Quaternion rotation = Quaternion.Euler(Vector3.up * Random.value * 360);
                Object.Instantiate(prefabs[Random.Range(0, prefabs.Length)], objPos, rotation, roomParent);
            }
        }

        private void CreateWall(Vector3 offset, Vector3 rotation, float size) {
            float wallHeight = 2;
            float wallWidth = 0.1f;
            Transform wall = Object.Instantiate(roomConfig.wallPrefab, roomPosition + offset, Quaternion.Euler(rotation), roomParent);
            wall.localScale = new Vector3(size, wallHeight, wallWidth);
        }

        private void CreateLight(Vector3 offset) {
            lights.Add(Object.Instantiate(roomConfig.lightPrefab, roomPosition + offset, Quaternion.identity, roomParent));
        }

        private Vector3 GetValidSpawnPosition() {
            Vector3 newPos;
            int attempt = 0;
            int maxAttempts = 10;
            do {
                if (attempt > maxAttempts) return Vector3.zero;
                attempt++;

                newPos = roomPosition + new Vector3(
                        Random.Range(-sizeHalf.x + roomConfig.offsetFromWalls, sizeHalf.x - roomConfig.offsetFromWalls),
                        0,
                        Random.Range(-sizeHalf.y + roomConfig.offsetFromWalls, sizeHalf.y - roomConfig.offsetFromWalls)
                    );
            }
            while (IsPositionOccupied(newPos, roomConfig.minDistanceBetweenObjects));

            return newPos;
        }

        private bool IsPositionOccupied(Vector3 pos, float minDistance) {
            foreach (var occupiedPos in occupiedPositions) {
                if (Vector3.Distance(pos, occupiedPos) < minDistance) {
                    return true;
                }
            }
            return false;
        }
    }

}
