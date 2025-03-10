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

        private Transform playerTransform;
        private CancellationToken cancellationToken;

        [Inject]
        public void Construct(PlayerController player, LevelController levelController, CancellationToken cancellationToken) {
            playerTransform = player.transform;
            this.cancellationToken = cancellationToken;

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
            foreach (Image icon in roomIconList) {
                Destroy(icon.gameObject);
            }
            roomIconList.Clear();
        }

        private void CreateMiniMap(List<RoomInfo> roomInfoList) {
            ClearMiniMap();
            this.roomInfoList = roomInfoList;

            foreach (RoomInfo info in roomInfoList) {
                Image roomIcon = Instantiate(roomIconPrefab, content);
                roomIcon.transform.localPosition = new Vector3(info.roomPosition.x * scale, info.roomPosition.z * scale, 0);
                roomIcon.GetComponent<RectTransform>().sizeDelta = (Vector3)info.size * scale;

                roomIconList.Add(roomIcon);
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