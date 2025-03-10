using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Project.Enemy {
    public class Enemy1 : EnemyBase {
        protected override void Attack() {
            AttackAsync().Forget();
        }

        private async UniTaskVoid AttackAsync() {
            isAttacking = true;

            await UniTask.Delay((int)(1000 * attackDuration / 2), cancellationToken: cancellationToken);

            Quaternion targetRotation = Quaternion.LookRotation(playerDirection);
            transform.rotation = targetRotation;

            InstantiateProjectile(targetRotation);
            Reload().Forget();
            isAttacking = false;
        }
    }
}