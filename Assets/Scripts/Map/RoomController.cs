using UnityEngine;
using Cysharp.Threading.Tasks;
using Project.Enemy;
using System.Threading;
using Zenject;

namespace Project.Map {

    public class RoomController : MonoBehaviour {

        private RoomInfo roomInfo;

        private CancellationToken cancellationToken;

        [Inject]
        public void Construct(CancellationToken cancellationToken) {
            this.cancellationToken = cancellationToken;
        }

        public void SetInfo(RoomInfo roomInfo) {
            this.roomInfo = roomInfo;

            foreach (var enemy in roomInfo.enemies) {
                enemy.gameObject.SetActive(false);
            }

            foreach (var portal in roomInfo.portals) {
                portal.gameObject.SetActive(false);
            }
        }

        private void Update() {
            if(roomInfo.isActivated && !roomInfo.isCleared) {
                if (!roomInfo.enemies.Exists(enemy => enemy != null)) {
                    RoomCleared();
                }
            }
        }

        void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player") && !roomInfo.isActivated) {
                StartRoom().Forget();
            }
        }

        private async UniTaskVoid StartRoom() {
            roomInfo.isActivated = true;

            await UniTask.Delay(1000, cancellationToken: cancellationToken);

            foreach (EnemyBase enemy in roomInfo.enemies) {
                enemy.gameObject.SetActive(true);
            }
        }

        private void RoomCleared() {
            roomInfo.isCleared = true;

            foreach (var light in roomInfo.lights) {
                light.color = Color.white;
            }

            foreach (var portal in roomInfo.portals) {
                portal.gameObject.SetActive(true);
            }
        }


    }
}