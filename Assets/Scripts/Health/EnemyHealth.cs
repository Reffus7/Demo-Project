
namespace Project.HealthSpace {

    public class EnemyHealth : Health {

        public void InitMaxHealth(int maxHealth) {
            this.maxHealth = maxHealth;
            health = maxHealth;
        }

    }
}