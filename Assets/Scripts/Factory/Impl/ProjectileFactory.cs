using Project.Factory;
using UnityEngine;
using Zenject;

public class ProjectileFactory : Factory, IInitializable {

    private Projectile projectilePrefab;
    private ObjectPool objectPool;
    private LevelController levelController;

    [Inject]
    public void Construct(Projectile projectilePrefab, LevelController levelController) {
        this.projectilePrefab = projectilePrefab;
        this.levelController = levelController;
    }

    public void Initialize() {
        objectPool = new ObjectPool(zenjectInstantiator, projectilePrefab.gameObject);
        levelController.onLevelChanged += _ => objectPool.ReturnAll();

    }

    public GameObject Create(int damage, float projectileSpeed, bool disableBounce) {
        GameObject projectileGO = objectPool.Get();
        Projectile projectile = projectileGO.GetComponent<Projectile>();
        projectile.Init(damage, projectileSpeed, disableBounce, objectPool);

        return projectileGO;
    }
}