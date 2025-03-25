using UnityEngine;
using Cysharp.Threading.Tasks;
using Project.Enemy;
using System.Threading;
using Zenject;
using Project.Factory;
using System.Collections.Generic;

namespace Project.Map {

    public class RoomController : MonoBehaviour {

        private RoomInfo roomInfo;

        private CancellationToken cancellationToken;

        [Inject]
        public void Construct(CancellationToken cancellationToken, EnemyFactory enemyFactory) {
            this.cancellationToken = cancellationToken;
            this.enemyFactory = enemyFactory;
        }

        public void SetInfo(RoomInfo roomInfo) {
            this.roomInfo = roomInfo;

            foreach (var portal in roomInfo.portals) {
                portal.gameObject.SetActive(false);
            }
        }

        private void Update() {
            if (roomInfo.isActivated && !roomInfo.isCleared) {
                if (!enemies.Exists(enemy => enemy.isAlive)) {
                    RoomCleared();
                }
            }
        }

        void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player") && !roomInfo.isActivated) {
                StartRoom().Forget();
            }
        }

        private EnemyFactory enemyFactory;

        private List<EnemyBase> enemies = new();
        private async UniTaskVoid StartRoom() {
            foreach (Vector3 enemyPos in roomInfo.enemyPositions) {
                EnemyBase enemy = await enemyFactory.Create();
                enemy.transform.position = enemyPos;
                enemy.gameObject.SetActive(false);
                enemies.Add(enemy);
            }
            roomInfo.isActivated = true;

            await UniTask.Delay(1000, cancellationToken: cancellationToken);

            foreach (EnemyBase enemy in enemies) { 
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