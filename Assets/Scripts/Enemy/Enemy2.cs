using Cysharp.Threading.Tasks;
using UnityEngine;


namespace Project.Enemy {
    public class Enemy2 : EnemyBase {
        protected override void Attack() {
            AttackAsync().Forget();

        }

        private async UniTaskVoid AttackAsync() {
            isAttacking = true;

            await UniTask.Delay((int)(1000 * attackDuration / 2), cancellationToken: cancellationToken);

            float angleStep = 15f;

            for (int i = -1; i <= 1; i++) {
                Quaternion rotation = Quaternion.Euler(0, angleStep * i, 0) * transform.rotation;
                InstantiateProjectile(rotation);
            }

            Reload().Forget();

            isAttacking = false;
        }
    }
}