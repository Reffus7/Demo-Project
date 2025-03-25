using System;
using UnityEngine;

namespace Project.HealthSpace {

    public class Health : MonoBehaviour {
        [SerializeField] protected int maxHealth;

        private bool isInvincible;

        protected int health;

        public event Action OnDie;
        public event Action<int> OnHealthChanged;
        public event Action<bool> OnInvincibilityChanged;

        protected void CallOnHealthChanged(int health) {
            OnHealthChanged?.Invoke(health);
        }

        public int GetMaxHealth() {
            return maxHealth;
        }

        private void Start() {
            health = maxHealth;
        }

        public void SetInvincibility(bool isInvincible) {
            this.isInvincible = isInvincible;
            OnInvincibilityChanged?.Invoke(isInvincible);
        }

        public void TakeDamage(int damage) {
            if (isInvincible) {
                return;
            }

            health -= damage;
            if (health <= 0) {
                health = 0;
                OnDie?.Invoke();
            }
            OnHealthChanged?.Invoke(health);

        }
    }
}