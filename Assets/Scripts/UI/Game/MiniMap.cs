using Cysharp.Threading.Tasks;
using Project.Map;
using Project.Player;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.UI {

    public class MiniMap : MonoBehaviour {
        [SerializeField] private RectTransform content;
        [SerializeField] private Image roomIconPrefab;
        [SerializeField] private float scale = 5f;

        private List<Image> roomIconList = new();
        private List<RoomInfo> roomInfoList = new();
        private ObjectPool objectPool = null;

        private Transform playerTransform;
        private CancellationToken cancellationToken;

        [Inject]
        public void Construct(
            PlayerController player,
            LevelController levelController,
            CancellationToken cancellationToken,
            ZenjectInstantiator zenjectInstantiator

        ) {
            playerTransform = player.transform;
            this.cancellationToken = cancellationToken;

            objectPool = new ObjectPool(zenjectInstantiator, roomIconPrefab.gameObject);


            levelController.onPrepareMap += CreateMiniMap;
        }


        private void Start() {
            CheckRoomClearedAsync().Forget();
        }

        void Update() {
            Vector3 playerPosition = playerTransform.position;
            content.localPosition = new Vector3(-playerPosition.x * scale, -playerPosition.z * scale, 0);

        }

        private void ClearMiniMap() {
            roomIconList.Clear();
            objectPool.ReturnAll();
        }

        private void CreateMiniMap(List<RoomInfo> roomInfoList) {
            if (objectPool == null) Debug.LogError("pool is null");

            ClearMiniMap();
            this.roomInfoList = roomInfoList;
            foreach (RoomInfo info in roomInfoList) {
                GameObject roomIconGO = objectPool.Get();
                Image roomIconImage = roomIconGO.GetComponent<Image>();
                roomIconImage.color = Color.red;
                roomIconGO.transform.SetParent(content);
                roomIconGO.transform.localPosition = new Vector3(info.roomPosition.x * scale, info.roomPosition.z * scale, 0);
                roomIconGO.GetComponent<RectTransform>().sizeDelta = (Vector3)info.size * scale;

                roomIconList.Add(roomIconImage);
            }
        }

        private float checkDelay = .25f;

        private async UniTaskVoid CheckRoomClearedAsync() {
            for (int i = 0; i < roomInfoList.Count; i++) {

                if (roomInfoList[i].isCleared) {
                    roomIconList[i].color = Color.white;
                }
            }

            await UniTask.Delay((int)(1000 * checkDelay), cancellationToken: cancellationToken);

            CheckRoomClearedAsync().Forget();

        }

    }
}