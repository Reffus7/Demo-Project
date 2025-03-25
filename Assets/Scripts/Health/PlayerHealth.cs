using Cysharp.Threading.Tasks;
using Project.Config;
using Project.Data;
using Project.Progress;
using Zenject;

namespace Project.HealthSpace {
    public class PlayerHealth : Health {

        private AssetReferenceContainer assetReferenceContainer;
        private AssetProvider assetProvider;
        private IDataSaver dataSaver;
        LevelController levelController;

        [Inject]
        public void Construct(
            IDataSaver dataSaver,
            AssetReferenceContainer assetReferenceContainer,
            AssetProvider assetProvider,
            LevelController levelController
        ) {
            this.dataSaver = dataSaver;
            this.assetReferenceContainer = assetReferenceContainer;
            this.assetProvider = assetProvider;
            this.levelController = levelController;

            InitAsync().Forget();

        }

        private async UniTaskVoid InitAsync() {
            levelController.onLevelChanged += _ => RestoreHealth();

            PlayerConfig config = await assetProvider.LoadAssetAsync<PlayerConfig>(assetReferenceContainer.playerConfig);

            PlayerProgressVar maxHealthProgress = config.maxHealthProgress;
            maxHealthProgress.SetLevel(dataSaver.GetPlayerParameterLevels().GetLevel(PlayerParameterType.maxHealth));
            maxHealth = (int)maxHealthProgress.Value;
            health = maxHealth;
        }

        public void RestoreHealth() {

            health = maxHealth;
            CallOnHealthChanged(health);
        }

    }
}