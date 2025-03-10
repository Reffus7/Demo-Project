using Cysharp.Threading.Tasks;
using Project.Config;
using Project.Enemy;
using Project.HealthSpace;
using Project.Progress;
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

        private async UniTaskVoid InitAsync() {
            RoomConfig roomConfig = await assetProvider.LoadAssetAsync<RoomConfig>(assetReferenceContainer.roomConfig);
            prefabs = roomConfig.enemyPrefabs;

            enemyConfig = await assetProvider.LoadAssetAsync<EnemyConfig>(assetReferenceContainer.enemyConfig);
            maxHealthProgress = enemyProgressVarFactory.Create(enemyConfig.healthMaxProgress);

        }


        // будет использовать objectpool, если он пуст, то фабрика создает.
        // так же фабрика возвращает в пул. пул по большей мере для хранения
        // сброс настроек тоже будет в фабрике
        public async UniTask<EnemyBase> Create() {
            await UniTask.WaitWhile(() => prefabs == null || enemyConfig == null);

            EnemyBase prefab = prefabs[Random.Range(0, prefabs.Length)];

            EnemyBase enemy = zenjectInstantiator.Instantiate(prefab).GetComponent<EnemyBase>();
            enemy.Init(enemyConfig);

            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            enemyHealth.InitMaxHealth((int)maxHealthProgress.Value);

            EnemyAnimator enemyAnimator = enemy.GetComponent<EnemyAnimator>();
            enemyAnimator.InitAttackSpeed(enemy.GetAttackSpeed());

            return enemy;

        }

    }
}