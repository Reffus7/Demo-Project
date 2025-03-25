using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using Zenject;
using Project.HealthSpace;
using Project.Config;
using System.Threading;
using Project.Data;
using Project.Progress;
using Project.Input;

namespace Project.Player {

    [RequireComponent(typeof(Rigidbody), typeof(PlayerHealth))]
    public class PlayerController : MonoBehaviour {
        [SerializeField] private LayerMask collisionMask;
        [SerializeField] private LayerMask enemyLayer;

        private float moveSpeed => moveSpeedProgress.Value;
        private float rotationSpeed => rotationSpeedProgress.Value;
        private float dodgeDistance => dodgeDistanceProgress.Value;
        private float dodgeDuration => dodgeDurationProgress.Value;
        private float dodgeInvincibility => dodgeInvincibilityProgress.Value;
        private float dodgeCooldown => dodgeCooldownProgress.Value;
        private float attackRange => attackRangeProgress.Value;
        private float attackSpeed => attackSpeedProgress.Value;
        private int attackDamage => (int)attackDamageProgress.Value;

        private PlayerProgressVar moveSpeedProgress;
        private PlayerProgressVar rotationSpeedProgress;
        private PlayerProgressVar dodgeDistanceProgress;
        private PlayerProgressVar dodgeDurationProgress;
        private PlayerProgressVar dodgeInvincibilityProgress;
        private PlayerProgressVar dodgeCooldownProgress;
        private PlayerProgressVar attackRangeProgress;
        private PlayerProgressVar attackSpeedProgress;
        private PlayerProgressVar attackDamageProgress;

        public event Action onDodge;
        public event Action onAttack;

        private Rigidbody rb;
        private PlayerHealth health;

        private Vector2 inputMovement;
        private bool isDodging = false;
        private bool canDodge = true;
        private bool isAttacking = false;

        private IInputHandler inputHandler;
        private CancellationToken cancellationToken;
        private IDataSaver dataSaver;


        [Inject]
        public void Construct(
            IInputHandler inputHandler,
            IDataSaver dataSaver,
            CancellationToken cancellationToken

        ) {

            this.inputHandler = inputHandler;
            this.dataSaver = dataSaver;
            this.cancellationToken = cancellationToken;

        }

        public void Init(PlayerConfig playerConfig) {
            moveSpeedProgress = playerConfig.moveSpeedProgress;
            rotationSpeedProgress = playerConfig.rotationSpeedProgress;
            dodgeDistanceProgress = playerConfig.dodgeDistanceProgress;
            dodgeDurationProgress = playerConfig.dodgeDurationProgress;
            dodgeInvincibilityProgress = playerConfig.dodgeInvincibilityProgress;
            dodgeCooldownProgress = playerConfig.dodgeCooldownProgress;
            attackRangeProgress = playerConfig.attackRangeProgress;
            attackSpeedProgress = playerConfig.attackSpeedProgress;
            attackDamageProgress = playerConfig.attackDamageProgress;

            PlayerParameterLevels levels = dataSaver.GetPlayerParameterLevels();

            moveSpeedProgress.SetLevel(levels.GetLevel(PlayerParameterType.moveSpeed));
            rotationSpeedProgress.SetLevel(levels.GetLevel(PlayerParameterType.rotationSpeed));
            dodgeDistanceProgress.SetLevel(levels.GetLevel(PlayerParameterType.dodgeDistance));
            dodgeDurationProgress.SetLevel(levels.GetLevel(PlayerParameterType.dodgeDuration));
            dodgeInvincibilityProgress.SetLevel(levels.GetLevel(PlayerParameterType.dodgeInvincibility));
            dodgeCooldownProgress.SetLevel(levels.GetLevel(PlayerParameterType.dodgeCooldown));
            attackRangeProgress.SetLevel(levels.GetLevel(PlayerParameterType.attackRange));
            attackSpeedProgress.SetLevel(levels.GetLevel(PlayerParameterType.attackSpeed));
            attackDamageProgress.SetLevel(levels.GetLevel(PlayerParameterType.attackDamage));
            attackDamageProgress.SetLevel(levels.GetLevel(PlayerParameterType.attackDamage));

        }

        private void Start() {

            rb = GetComponent<Rigidbody>();
            health = GetComponent<PlayerHealth>();
            health.OnDie += HandleDeath;

            inputHandler.onMove += ReadMove;
            inputHandler.onDodge += Dodge;
            inputHandler.onAttack += Attack;
        }

        private void OnDestroy() {
            if (health != null) health.OnDie -= HandleDeath;
            if (inputHandler != null) {
                inputHandler.onMove -= ReadMove;
                inputHandler.onDodge -= Dodge;
                inputHandler.onAttack -= Attack;
            }

        }

        private void ReadMove(Vector2 movementVector) {
            inputMovement = movementVector;
        }

        private void Dodge() {


            DodgeAsync().Forget();
        }


        private void Attack() {

            AttackAsync().Forget();
        }

        private void FixedUpdate() {
            Move();
        }

        private void Update() {
            Rotate();
        }

        public float GetDodgeDuration() {
            return dodgeCooldown;
        }

        public float GetAttackSpeed() {
            return attackSpeed;
        }

        private void HandleDeath() {
            inputHandler.DisableInput();
            inputMovement = Vector2.zero;
        }

        private void Move() {
            if (isDodging) return;

            rb.velocity = new Vector3(inputMovement.x, 0, inputMovement.y) * moveSpeed;
        }

        private void Rotate() {
            if (isDodging) return;
            if (inputMovement == Vector2.zero) return;

            Vector3 direction = new Vector3(inputMovement.x, 0, inputMovement.y);
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        private async UniTaskVoid DodgeAsync() {
            if (!canDodge) return;
            canDodge = false;

            health.SetInvincibility(true);
            isDodging = true;

            onDodge?.Invoke();

            UniTask taskInvicibility = UniTask.Delay((int)(dodgeInvincibility * 1000), cancellationToken: cancellationToken);
            UniTask taskDodgeCooldown = UniTask.Delay((int)(dodgeCooldown * 1000), cancellationToken: cancellationToken);

            Vector3 dodgeDirection = transform.forward.normalized;
            Vector3 targetPosition = rb.position + dodgeDirection * dodgeDistance;

            float elapsedTime = 0;
            Vector3 startPosition = rb.position;

            while (elapsedTime < dodgeDuration) {
                if (Physics.Raycast(rb.position + Vector3.up, dodgeDirection, dodgeDistance / 4, collisionMask)) {
                    isDodging = false;
                    break;
                }
                rb.MovePosition(Vector3.Lerp(startPosition, targetPosition, elapsedTime / dodgeDuration));
                elapsedTime += Time.deltaTime;
                await UniTask.Yield();
            }

            isDodging = false;

            await taskInvicibility;

            health.SetInvincibility(false);

            await taskDodgeCooldown;

            canDodge = true;

        }

        private async UniTaskVoid AttackAsync() {
            if (isAttacking) return;
            isAttacking = true;

            onAttack?.Invoke();

            float duration = 1 / attackSpeed;
            float halfDuration = duration / 2;



            UniTask halfDurationTask = UniTask.Delay((int)(halfDuration * 1000), cancellationToken: cancellationToken);
            UniTask fullDurationTask = UniTask.Delay((int)(duration * 1000), cancellationToken: cancellationToken);

            await halfDurationTask;

            PerformAttack();

            await fullDurationTask;

            isAttacking = false;

        }

        void OnDrawGizmosSelected() {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + transform.forward * attackRange / 2, attackRange / 2);
        }

        private void PerformAttack() {

            Collider[] hitEnemies = Physics.OverlapSphere(transform.position + transform.forward * attackRange / 2, attackRange / 2, enemyLayer);

            foreach (var enemy in hitEnemies) {
                Health enemyHealth = enemy.GetComponent<Health>();
                if (enemyHealth != null) {
                    enemyHealth.TakeDamage(attackDamage);
                }

            }
        }


    }
}