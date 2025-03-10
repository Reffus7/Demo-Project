using Cysharp.Threading.Tasks;
using Project.HealthSpace;
using System.Threading;
using UnityEngine;
using Zenject;

namespace Project.Player {
    [RequireComponent(typeof(PlayerController), typeof(Animator))]
    public class PlayerAnimator : MonoBehaviour {
        private const float attackAnimationNormalizeCoeff = 3.003f;
        private const string animationParameterAttackSpeed = "AttackSpeed";

        // Components
        private PlayerController playerController;
        private Animator animator;
        private PlayerHealth playerHealth;

        SkinnedMeshRenderer[] skins;

        private IInputHandler inputHandler;
        private CancellationToken cancellationToken;

        [Inject]
        public void Construct(IInputHandler inputHandler, CancellationToken cancellationToken) {
            this.inputHandler = inputHandler;
            this.cancellationToken = cancellationToken;
        }

        private void Start() {
            playerController = GetComponent<PlayerController>();
            animator = GetComponent<Animator>();
            playerHealth = GetComponent<PlayerHealth>();

            skins = GetComponentsInChildren<SkinnedMeshRenderer>();


            animator.SetFloat(animationParameterAttackSpeed, playerController.GetAttackSpeed() * attackAnimationNormalizeCoeff);

            inputHandler.onMove += MoveAnimation;
            playerController.onAttack += AttackAnimation;
            playerHealth.OnDie += DieAnimation;
            playerHealth.OnHealthChanged += HitAnimation;

        }

        private void OnDisable() {
            if (inputHandler != null) inputHandler.onMove -= MoveAnimation;
            if (playerController != null) playerController.onAttack -= AttackAnimation;
            if (playerHealth != null) playerHealth.OnDie -= DieAnimation;
            if (playerHealth != null) playerHealth.OnHealthChanged -= HitAnimation;

        }

        private void DieAnimation() {
            animator.SetTrigger("Death");

            enabled = false;
        }

        private void HitAnimation(int _) {
            foreach (SkinnedMeshRenderer skin in skins) {
                HitAnimationAsync(skin).Forget();
            }
        }

        private async UniTaskVoid HitAnimationAsync(SkinnedMeshRenderer skin) {
            skin.material.color = Color.red;

            await UniTask.Delay(250, cancellationToken: cancellationToken);

            skin.material.color = Color.white;

        }

        private void MoveAnimation(Vector2 movementVector) {
            bool isMoving = movementVector != Vector2.zero;

            animator.SetBool("Run", isMoving);
            animator.SetBool("Idle", !isMoving);
        }

        private void AttackAnimation() {
            animator.SetTrigger("Attack");

        }



    }
}