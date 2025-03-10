using System.Collections;
using UnityEngine;

namespace Project.Map {
    public class Portal : MonoBehaviour {
        private Vector3 targetPosition;
        private bool isTeleporting = false;

        public void SetTarget(Vector3 target) {
            targetPosition = target;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player") && !isTeleporting) {
                isTeleporting = true;
                StartCoroutine(TeleportPlayer(other.transform));
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.CompareTag("Player")){
                isTeleporting = false;

            }
        }

        private IEnumerator TeleportPlayer(Transform player) {
            yield return new WaitForSeconds(0.5f);
            if (isTeleporting) {
                player.position = targetPosition;
            }
            isTeleporting = false;
        }
    }
}