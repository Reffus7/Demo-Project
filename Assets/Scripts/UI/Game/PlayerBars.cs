using Cysharp.Threading.Tasks;
using Project.HealthSpace;
using Project.Player;
using System.Threading;
using UnityEngine;
using Zenject;

namespace Project.UI {

    namespace Project.UI {
        public class PlayerBars : MonoBehaviour {

            [SerializeField] private RectTransform healthBar;
            [SerializeField] private RectTransform dodgeBar;

            private PlayerHealth playerHealth;

            private float healthBarWidth;
            private float dodgeBarWidth;

            private PlayerController playerController;
            private CancellationToken cancellationToken;

            [Inject]
            public void Construct(PlayerController playerController, CancellationToken cancellationToken) {
                this.playerController = playerController;
                this.cancellationToken = cancellationToken;

                playerHealth = playerController.GetComponent<PlayerHealth>();
                playerController.onDodge += UpdateDodgeUI;
                playerHealth.OnHealthChanged += UpdateHealthUI;
            }

            private void OnDestroy() {
                if (playerController != null) playerController.onDodge -= UpdateDodgeUI;
                if (playerHealth != null) playerHealth.OnHealthChanged -= UpdateHealthUI;
            }

            private void UpdateDodgeUI() {
                UpdateDodgeUIAsync().Forget();
            }

            private async UniTaskVoid UpdateDodgeUIAsync() {
                float dodgeDuration = playerController.GetDodgeDuration();
                float elapsedTime = 0;
                while (elapsedTime < dodgeDuration) {
                    if (cancellationToken.IsCancellationRequested) return;

                    UpdateBar(dodgeBar, dodgeBarWidth, elapsedTime / dodgeDuration);

                    elapsedTime += Time.deltaTime;
                    await UniTask.Yield();
                }

                UpdateBar(dodgeBar, dodgeBarWidth);
            }
            private void UpdateHealthUI(int health) {
                UpdateBar(healthBar, healthBarWidth, (float)health / playerHealth.GetMaxHealth());
            }

            private void UpdateBar(RectTransform bar, float width, float coeff = 1) {
                Vector2 size = bar.sizeDelta;
                size.x = width * coeff;
                bar.sizeDelta = size;
            }


            void Start() {
                healthBarWidth = healthBar.sizeDelta.x;
                dodgeBarWidth = dodgeBar.sizeDelta.x;
            }


        }
    }
}