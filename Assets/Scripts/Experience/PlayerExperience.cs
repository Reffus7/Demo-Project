using Cysharp.Threading.Tasks;
using Project.Config;
using Project.Data;
using Project.Enemy;
using Project.Factory;
using Project.HealthSpace;
using Project.Progress;
using Zenject;

public class PlayerExperience : IInitializable {
    private int currentExperience;
    private int currentPlayerLevel;

    private int nextLevelExperience => (int)playerLevelProgress.Value;
    private int enemyExperience => (int)enemyExperienceProgress.Value;

    private PlayerProgressVar playerLevelProgress;
    private EnemyProgressVar enemyExperienceProgress;

    private IDataSaver dataSaver;
    private EnemyProgressVarFactory enemyProgressVarFactory;
    private AssetReferenceContainer assetReferenceContainer;
    private AssetProvider assetProvider;
    private EnemyFactory enemyFactory;

    [Inject]
    public void Construct(
        IDataSaver dataSaver,
        EnemyProgressVarFactory enemyProgressVarFactory,
        AssetReferenceContainer assetReferenceContainer,
        AssetProvider assetProvider,
        EnemyFactory enemyFactory

    ) {

        this.dataSaver = dataSaver;
        this.enemyProgressVarFactory = enemyProgressVarFactory;
        this.assetProvider = assetProvider;
        this.assetReferenceContainer = assetReferenceContainer;
        this.enemyFactory = enemyFactory;
    }

    public void Initialize() {
        InitAsync().Forget();

    }

    private async UniTaskVoid InitAsync() {
        ExperienceConfig config = await assetProvider.LoadAssetAsync<ExperienceConfig>(assetReferenceContainer.experienceConfig);

        currentPlayerLevel = dataSaver.GetPlayerLevel();
        currentExperience = dataSaver.GetPlayerExperience();

        playerLevelProgress = config.playerLevelExperienceProgress;
        playerLevelProgress.SetLevel(currentPlayerLevel);

        enemyExperienceProgress = enemyProgressVarFactory.Create(config.enemyExperienceProgress);

        enemyFactory.onEnemyCreate += SubscribeOnEnemyDie;
    }

    private void SubscribeOnEnemyDie(EnemyBase enemy) {
        enemy.GetComponent<Health>().OnDie += GiveExperience;
    }

    private void GiveExperience() {
        currentExperience += enemyExperience;

        CheckLevel();
        SaveData();
    }

    private void CheckLevel() {
        if (currentExperience >= nextLevelExperience) {
            currentExperience -= nextLevelExperience;
            currentPlayerLevel++;
            playerLevelProgress.SetLevel(currentPlayerLevel);
        }
    }


    private void SaveData() {
        dataSaver.SavePlayerLevel(currentPlayerLevel);
        dataSaver.SavePlayerExperience(currentExperience);
    }


}