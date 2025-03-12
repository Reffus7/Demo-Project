using Project.HealthSpace;
using UnityEngine;

public class Projectile : MonoBehaviour {
    private ObjectPool objectPool;

    [SerializeField] private float moveSpeed;

    private bool isBounced;
    private int damage;

    private Rigidbody rb;

    public void Init(int damage, float moveSpeed, bool disableBounce, ObjectPool objectPool) {
        this.objectPool = objectPool;
        this.damage = damage;
        this.moveSpeed = moveSpeed;
        isBounced = disableBounce;
    }

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        rb.velocity = transform.forward * moveSpeed;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) {
            Health health = collision.gameObject.GetComponent<Health>();
            if (health != null) {
                health.TakeDamage(damage);
            }
            objectPool.Return(gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle")) {
            if (isBounced) {
                objectPool.Return(gameObject);

                return;
            }

            Vector3 normal = collision.contacts[0].normal;
            Vector3 direction = Vector3.Reflect(transform.forward, normal);
            transform.rotation = Quaternion.LookRotation(direction);

            isBounced = true;
        }
    }

}
