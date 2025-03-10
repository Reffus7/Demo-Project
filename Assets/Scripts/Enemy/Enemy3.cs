using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Project.Enemy {
    public class Enemy3 : EnemyBase {
        [SerializeField] private int shots = 6;
        protected override void Attack() {
            AttackAsync().Forget();
        }

        private async UniTaskVoid AttackAsync() {
            isAttacking = true;

            await UniTask.Delay((int)(1000 * attackDuration / 2), cancellationToken: cancellationToken);

            for (int i = 0; i < shots; i++) {
                float angle = 360f / shots * i;
                Quaternion rotation = Quaternion.Euler(0, angle, 0);
                InstantiateProjectile(rotation, disableBounce: true);
            }

            Reload().Forget();

            isAttacking = false;
        }

    }


}