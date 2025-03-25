using Cysharp.Threading.Tasks;
using Project.Config;
using Project.Factory;
using UnityEngine;
using Zenject;

namespace Project.Factory {

    public class ProjectileFactory : Factory, IInitializable {

        private ObjectPool objectPool;
        private LevelController levelController;
        private AssetReferenceContainer assetReferenceContainer;
        private AssetProvider assetProvider;

        [Inject]
        public void Construct(
            LevelController levelController,
            AssetReferenceContainer assetReferenceContainer,
            AssetProvider assetProvider

        ) {
            this.levelController = levelController;
            this.assetReferenceContainer = assetReferenceContainer;
            this.assetProvider = assetProvider;
        }

        public void Initialize() {
            InitAsync().Forget();

        }

        private async UniTaskVoid InitAsync() {
            GameObject projectileGO = await assetProvider.LoadAssetAsync<GameObject>(assetReferenceContainer.projectilePrefab);
            objectPool = new ObjectPool(zenjectInstantiator, projectileGO);

            levelController.onLevelChanged += _ => objectPool.ReturnAll();
        }

        public GameObject Create(int damage, float projectileSpeed, bool disableBounce) {
            GameObject projectileGO = objectPool.Get();
            Projectile projectile = projectileGO.GetComponent<Projectile>();
            projectile.Init(damage, projectileSpeed, disableBounce, objectPool);

            return projectileGO;
        }
    }
}