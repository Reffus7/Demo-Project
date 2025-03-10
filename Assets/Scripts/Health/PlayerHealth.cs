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

        [Inject]
        public void Construct(
            IDataSaver dataSaver,
            AssetReferenceContainer assetReferenceContainer,
            AssetProvider assetProvider
        ) {
            this.dataSaver = dataSaver;
            this.assetReferenceContainer = assetReferenceContainer;
            this.assetProvider = assetProvider;

            InitAsync().Forget();

        }


        private async UniTaskVoid InitAsync() {
            PlayerConfig config = await assetProvider.LoadAssetAsync<PlayerConfig>(assetReferenceContainer.playerConfig);

            PlayerProgressVar maxHealthProgress = config.maxHealthProgress;
            maxHealthProgress.SetLevel(dataSaver.GetPlayerParameterLevels().maxHealth);
            maxHealth = (int)maxHealthProgress.Value;
            health = maxHealth;
        }


        public void Heal(int heal) {
            health += heal;
            if (health > maxHealth) {
                health = maxHealth;
            }
        }
    }
}