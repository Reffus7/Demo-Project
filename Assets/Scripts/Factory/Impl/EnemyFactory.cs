using Cysharp.Threading.Tasks;
using Project.Config;
using Project.Enemy;
using Project.HealthSpace;
using Project.Progress;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Project.Factory {

    public class EnemyFactory : Factory, IInitializable {

        private EnemyBase[] prefabs;
        private EnemyProgressVar maxHealthProgress;
        private EnemyConfig enemyConfig;


        private EnemyProgressVarFactory enemyProgressVarFactory;
        private AssetReferenceContainer assetReferenceContainer;
        private AssetProvider assetProvider;

        [Inject]
        public void Construct(
            EnemyProgressVarFactory enemyProgressVarFactory,
            AssetReferenceContainer assetReferenceContainer,
            AssetProvider assetProvider

        ) {

            this.enemyProgressVarFactory = enemyProgressVarFactory;
            this.assetProvider = assetProvider;
            this.assetReferenceContainer = assetReferenceContainer;
        }

        public void Initialize() {
            InitAsync().Forget();

        }

        private List<ObjectPool> pools = new();

        private async UniTaskVoid InitAsync() {
            RoomConfig roomConfig = await assetProvider.LoadAssetAsync<RoomConfig>(assetReferenceContainer.roomConfig);
            prefabs = roomConfig.enemyPrefabs;

            enemyConfig = await assetProvider.LoadAssetAsync<EnemyConfig>(assetReferenceContainer.enemyConfig);
            maxHealthProgress = enemyProgressVarFactory.Create(enemyConfig.healthMaxProgress);

            foreach (EnemyBase prefab in prefabs) {
                pools.Add(new ObjectPool(zenjectInstantiator, prefab.gameObject));
            }
        }

        public async UniTask<EnemyBase> Create() {
            await UniTask.WaitWhile(() => prefabs == null || enemyConfig == null);

            int enemyType = Random.Range(0, pools.Count);
            ObjectPool currentPool = pools[enemyType];

            GameObject enemy = currentPool.Get();

            EnemyBase enemyComponent = enemy.GetComponent<EnemyBase>();
            enemyComponent.enabled = true;
            enemyComponent.Init(enemyConfig, currentPool);

            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            enemyHealth.InitMaxHealth((int)maxHealthProgress.Value);

            EnemyAnimator enemyAnimator = enemy.GetComponent<EnemyAnimator>();
            enemyAnimator.enabled = true;
            enemyAnimator.InitAttackSpeed(enemyComponent.GetAttackSpeed());

            return enemyComponent;

        }

    }
}