using Cysharp.Threading.Tasks;
using Project.HealthSpace;
using System.Threading;
using UnityEngine;
using Zenject;

namespace Project.Enemy {

    [RequireComponent(typeof(Animator))]
    public class EnemyAnimator : MonoBehaviour {


        private Animator animator;
        private EnemyBase enemyBase;
        private EnemyHealth health;

        SkinnedMeshRenderer[] skins;

        private CancellationToken cancellationToken;

        [Inject]
        public void Construct(CancellationToken cancellationToken) {
            this.cancellationToken = cancellationToken;
        }

        private void Awake() {
            animator = GetComponent<Animator>();
            enemyBase = GetComponent<EnemyBase>();
            health = GetComponent<EnemyHealth>();

            skins = GetComponentsInChildren<SkinnedMeshRenderer>();


        }

        private float attackSpeed;

        public void InitAttackSpeed(float attackSpeed) {
            this.attackSpeed = attackSpeed;

        }

        private void OnEnable() {
            enemyBase.onAttack += AttackAnimation;
            enemyBase.onMove += MoveAnimation;

            health.OnHealthChanged += HitAnimation;
            health.OnDie += DieAnimation;

        }

        private void OnDisable() {
            if (enemyBase != null) {
                enemyBase.onAttack -= AttackAnimation;
                enemyBase.onMove -= MoveAnimation;
            }
            if (health != null) {
                health.OnDie -= DieAnimation;
                health.OnHealthChanged -= HitAnimation;
            }
        }

        private void MoveAnimation(bool isMoving) {
            animator.SetBool("Walk", isMoving);
            animator.SetBool("Idle", !isMoving);
        }

        private void HitAnimation(int _) {
            animator.SetTrigger("Hit");
            foreach (SkinnedMeshRenderer skin in skins) {
                HitAnimationAsync(skin).Forget();
            }
        }

        private async UniTaskVoid HitAnimationAsync(SkinnedMeshRenderer skin) {
            skin.material.color = Color.red;

            await UniTask.Delay(250, cancellationToken: cancellationToken);

            skin.material.color = Color.white;
        }

        private void DieAnimation() {
            animator.SetTrigger("Death");
            foreach (SkinnedMeshRenderer skin in skins) {
                HitAnimationAsync(skin).Forget();
            }
            enemyBase.enabled = false;
            enabled = false;
        }

        private void AttackAnimation() {
            animator.SetFloat("AttackSpeed", attackSpeed);
            animator.SetTrigger("Attack");
        }

    }
}