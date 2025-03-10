using UnityEngine;
using System.Collections.Generic;
using Zenject;
using Project.Config;
using Project.Factory;
using Project.Progress;
using Cysharp.Threading.Tasks;

namespace Project.Map {

    public class MapGenerator : IInitializable {
        private List<RoomInfo> roomInfoList = new List<RoomInfo>();
        private Transform mapParent;

        private int roomCount => (int)roomCountProgress.Value;

        // from config
        private Portal portalPrefab;
        private EnemyProgressVar roomCountProgress;
        private int portalsInRoom;
        private int roomSpacing;

        //from construct
        private RoomGenerator roomGenerator;
        private EnemyProgressVarFactory enemyProgressVarFactory;
        private ZenjectInstantiator zenjectInstantiator;
        private AssetReferenceContainer assetReferenceContainer;
        private AssetProvider assetProvider;

        [Inject]
        public void Construct(
            RoomGenerator roomGenerator,
            EnemyProgressVarFactory enemyProgressVarFactory,
            ZenjectInstantiator zenjectInstantiator,
            AssetReferenceContainer assetReferenceContainer,
            AssetProvider assetProvider
        ) {

            this.roomGenerator = roomGenerator;
            this.enemyProgressVarFactory = enemyProgressVarFactory;
            this.zenjectInstantiator = zenjectInstantiator;
            this.assetReferenceContainer = assetReferenceContainer;
            this.assetProvider = assetProvider;
        }

        public void Initialize() {
            InitAsync().Forget();

        }

        private async UniTaskVoid InitAsync() {
            MapConfig mapConfig = await assetProvider.LoadAssetAsync<MapConfig>(assetReferenceContainer.mapConfig);

            portalPrefab = mapConfig.portalPrefab;
            portalsInRoom = mapConfig.portalsInRoom;

            roomCountProgress = enemyProgressVarFactory.Create(mapConfig.roomCountProgress);

            RoomConfig roomConfig = await assetProvider.LoadAssetAsync<RoomConfig>(assetReferenceContainer.roomConfig);

            roomSpacing = roomConfig.minMaxSize.y;
        }

        public List<RoomInfo> GetRoomInfoList() {
            return roomInfoList;
        }

        public void ClearMap() {
            if (mapParent != null) {
                Object.Destroy(mapParent.gameObject);
            }
            roomInfoList.Clear();
        }

        public async UniTask GenerateMap() {
            int xPositions = 1;
            int zPositions = 1;

            while (xPositions * zPositions < roomCount) {
                if (xPositions > zPositions) zPositions++;
                else xPositions++;
            }

            for (int i = 0; i < roomCount; i++) {
                Vector3 newRoomPos;
                do {
                    newRoomPos = new Vector3(
                        Random.Range(0, xPositions) * roomSpacing,
                        0,
                        Random.Range(0, zPositions) * roomSpacing
                    );
                } while (roomInfoList.Exists(room => room.roomPosition == newRoomPos));
                RoomInfo roomInfo = await roomGenerator.GenerateRoom(newRoomPos);
                roomInfoList.Add(roomInfo);
            }

            CreatePortals();

            mapParent = new GameObject("Map").transform;

            foreach (RoomInfo roomInfo in roomInfoList) {
                ConfigureRoom(roomInfo);
            }

        }

        private void ConfigureRoom(RoomInfo roomInfo) {
            roomInfo.roomParent.SetParent(mapParent);

            GameObject roomParentGO= roomInfo.roomParent.gameObject;

            RoomController roomController= roomParentGO.AddComponent<RoomController>();
            roomController.SetInfo(roomInfo);
            zenjectInstantiator.Inject(roomController);

            BoxCollider collider = roomParentGO.AddComponent<BoxCollider>();
            collider.size = new Vector3(roomInfo.size.x, 1, roomInfo.size.y);
            collider.isTrigger = true;
        }

        private void CreatePortals() {
            foreach (RoomInfo roomInfo in roomInfoList) {
                List<Portal> portals = new List<Portal>();
                foreach (Vector3 targetPos in FindClosestRooms(roomInfo.roomPosition, portalsInRoom)) {
                    Vector3 direction = (targetPos - roomInfo.roomPosition).normalized;

                    Vector3 portalPos;
                    float xCoeff = Mathf.Abs(roomInfo.size.x / 2 / direction.x) - 1;
                    float zCoeff = Mathf.Abs(roomInfo.size.y / 2 / direction.z) - 1;

                    if (xCoeff < zCoeff) {
                        portalPos = roomInfo.roomPosition + direction * xCoeff;
                    }
                    else {
                        portalPos = roomInfo.roomPosition + direction * zCoeff;
                    }

                    float portalSpacing = 1;
                    bool positionOccupied = false;
                    foreach (Portal existingPortal in portals) {
                        if (Vector3.Distance(portalPos, existingPortal.transform.position) < portalSpacing) {
                            positionOccupied = true;
                            break;
                        }
                    }

                    if (positionOccupied) {
                        Vector3 shiftDirection = (roomInfo.roomPosition - portalPos).normalized;
                        portalPos += shiftDirection * portalSpacing;
                    }

                    Portal portal = Object.Instantiate(portalPrefab, portalPos, Quaternion.identity, roomInfo.roomParent);
                    portal.SetTarget(targetPos);

                    portals.Add(portal);
                }
                roomInfo.portals = portals;
            }
        }

        private List<Vector3> FindClosestRooms(Vector3 roomPos, int maxConnections) {
            List<Vector3> closestRooms = new List<Vector3>();

            foreach (RoomInfo room in roomInfoList) {
                if (room.roomPosition != roomPos) {
                    closestRooms.Add(room.roomPosition);
                }
            }

            closestRooms.Sort((a, b) => Vector3.Distance(roomPos, a).CompareTo(Vector3.Distance(roomPos, b)));

            return closestRooms.GetRange(0, Mathf.Min(maxConnections, closestRooms.Count));
        }


    }


}