using Project.HealthSpace;
using UnityEngine;

public class Projectile : MonoBehaviour {
    [SerializeField] private float moveSpeed;

    private bool isBounced;
    private int damage;

    private Rigidbody rb;

    public void DisableBounce() {
        isBounced = true;
    }

    public void SetDamage(int damage) {
        this.damage = damage;
    }

    public void SetSpeed(float speed) {
        moveSpeed = speed;
    }

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        rb.velocity = transform.forward * moveSpeed;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) {
            Health health= collision.gameObject.GetComponent<Health>();
            if (health != null) {
                health.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle")) {
            if (isBounced) {
                Destroy(gameObject);
                return;
            }

            Vector3 normal = collision.contacts[0].normal;
            Vector3 direction = Vector3.Reflect(transform.forward, normal);
            transform.rotation = Quaternion.LookRotation(direction);

            isBounced = true;
        }
    }

}
