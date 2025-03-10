using Cysharp.Threading.Tasks;
using Project.Config;
using Project.Factory;
using Project.HealthSpace;
using Project.Player;
using Project.Progress;
using System.Threading;
using UnityEngine;
using Zenject;

namespace Project.Enemy {
    [RequireComponent(typeof(Health), typeof(Rigidbody))]
    public abstract class EnemyBase : MonoBehaviour {
        private float attackRange;
        private float fleeRange;

        private float projectileSpeed => projectileSpeedProgress.Value;
        private float reloadTime => reloadTimeProgress.Value;
        private float moveSpeed => moveSpeedProgress.Value;
        private float rotateSpeed => rotateSpeedProgress.Value;
        private int damage => (int)damageProgress.Value;
        private float attackSpeed => attackSpeedProgress.Value;

        private EnemyProgressVar projectileSpeedProgress;
        private EnemyProgressVar reloadTimeProgress;
        private EnemyProgressVar moveSpeedProgress;
        private EnemyProgressVar rotateSpeedProgress;
        private EnemyProgressVar damageProgress;
        private EnemyProgressVar attackSpeedProgress;

        protected float attackDuration => 1.15f / attackSpeed;

        public event System.Action onAttack;
        public event System.Action<bool> onMove;

        protected Vector3 playerDirection => playerTransform.position - transform.position;

        protected bool isReloading = false;
        protected bool isAttacking;

        //components
        private Rigidbody rb;
        private EnemyHealth health;

        // construct
        protected Transform playerTransform;
        protected CancellationToken cancellationToken;
        private Projectile projectilePrefab;
        private EnemyProgressVarFactory enemyProgressVarFactory;


        [Inject]
        public void Construct(
            PlayerController playerController,
            Projectile projectilePrefab,
            EnemyProgressVarFactory enemyProgressVarFactory,
            CancellationToken cancellationToken

        ) {

            playerTransform = playerController.transform;
            this.cancellationToken = cancellationToken;
            this.projectilePrefab = projectilePrefab;
            this.enemyProgressVarFactory = enemyProgressVarFactory;
        }

        public void Init(EnemyConfig enemyConfig) { 

            attackRange = enemyConfig.attackRange;
            fleeRange = enemyConfig.fleeRange;

            projectileSpeedProgress = enemyProgressVarFactory.Create(enemyConfig.projectileSpeedProgress);
            reloadTimeProgress = enemyProgressVarFactory.Create(enemyConfig.reloadTimeProgress);
            moveSpeedProgress = enemyProgressVarFactory.Create(enemyConfig.moveSpeedProgress);
            rotateSpeedProgress = enemyProgressVarFactory.Create(enemyConfig.rotateSpeedProgress);
            damageProgress = enemyProgressVarFactory.Create(enemyConfig.damageProgress);
            attackSpeedProgress = enemyProgressVarFactory.Create(enemyConfig.attackSpeedProgress);
        }


        private void Start() {
            rb = GetComponent<Rigidbody>();
            health = GetComponent<EnemyHealth>();
            health.OnDie += HandleDeath;
            targetPosition = transform.position;
        }

        private void OnDestroy() {
            if (health != null) health.OnDie -= HandleDeath;
        }

        private void HandleDeath() {
            Destroy(gameObject, 1);
        }

        public float GetAttackSpeed() {
            return attackSpeed;
        }


        private void FixedUpdate() {
            if (playerTransform == null) return;

            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            if (!isReloading) {
                if (distanceToPlayer <= attackRange) {
                    if (RotatedToDirection(playerDirection) && !isAttacking) {
                        Attack();
                        onAttack?.Invoke();

                    }

                    SmoothRotate(playerDirection);
                }
                else {
                    Patrol();
                }
            }
            else {
                if (distanceToPlayer <= fleeRange) {
                    Flee();
                }
                else {
                    Patrol();
                }
            }

            if (rb.velocity.magnitude > 0) {
                onMove?.Invoke(true);
            }
            else {
                onMove?.Invoke(false);
            }
        }

        protected abstract void Attack();

        protected void Flee() {
            MoveWithObstacleAvoidance(-playerDirection);
        }

        private Vector3 CalculateNewDirection(Vector3 direction) {
            RaycastHit hit;
            float rayDistance = 1f;

            if (Physics.Raycast(transform.position + Vector3.up, direction, out hit, rayDistance)) {
                Vector3[] rayDirections = new Vector3[] {
            Quaternion.Euler(0, 45, 0) * direction,
            Quaternion.Euler(0, -45, 0) * direction,
            Quaternion.Euler(0, 90, 0) * direction,
            Quaternion.Euler(0, -90, 0) * direction
        };

                foreach (var rayDir in rayDirections) {
                    if (!Physics.Raycast(transform.position + Vector3.up, rayDir, rayDistance)) {
                        return rayDir;
                    }
                }

                return Vector3.Reflect(direction, hit.normal);
            }

            return direction;
        }

        private void MoveWithObstacleAvoidance(Vector3 direction) {
            Vector3 newDirection = CalculateNewDirection(direction);
            Move(newDirection);
        }

        private Vector3 targetPosition;
        private float changeDirectionTime = 0f;
        private float timer = 0f;

        protected void Patrol() {
            timer += Time.deltaTime;

            if (timer >= changeDirectionTime) {
                Vector3 randomDirection = Random.onUnitSphere;
                randomDirection.y = 0;
                randomDirection.Normalize();

                targetPosition = transform.position + randomDirection * 3;
                timer = 0f;
                changeDirectionTime = Random.Range(3, 5);
            }

            if (Vector3.Distance(targetPosition, transform.position) > .1f) {
                Move(targetPosition - transform.position);

            }
        }

        private bool RotatedToDirection(Vector3 direction) {
            direction.y = 0;
            direction.Normalize();
            if (Vector3.Distance(direction, transform.forward) < .1f) {
                return true;
            }
            return false;
        }

        protected void Move(Vector3 direction) {
            direction.y = 0;
            direction.Normalize();

            if (RotatedToDirection(direction)) {
                rb.velocity = direction * moveSpeed;
            }

            SmoothRotate(direction);
        }


        private void SmoothRotate(Vector3 direction) {
            direction.y = 0;
            direction.Normalize();

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
        }

        protected void InstantiateProjectile(Quaternion rotation, bool disableBounce = false) {
            Projectile projectile = Instantiate(projectilePrefab, transform.position + Vector3.up, rotation);
            projectile.SetDamage(damage);
            projectile.SetSpeed(projectileSpeed);
            if (disableBounce) projectile.DisableBounce();
        }

        protected async UniTask Reload() {
            isReloading = true;

            await UniTask.WaitForSeconds(reloadTime);

            isReloading = false;
        }

    }
}
