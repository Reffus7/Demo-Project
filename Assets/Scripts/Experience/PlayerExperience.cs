using Cysharp.Threading.Tasks;
using Project.Config;
using Project.Data;
using Project.Enemy;
using Project.Factory;
using Project.HealthSpace;
using Project.Map;
using Project.Progress;
using System.Collections.Generic;
using Zenject;

public class PlayerExperience : IInitializable {
    private int currentExperience;
    private int currentPlayerLevel;

    private int nextLevelExperience => (int)playerLevelProgress.Value;
    private int enemyExperience => (int)enemyExperienceProgress.Value;

    private PlayerProgressVar playerLevelProgress;
    private EnemyProgressVar enemyExperienceProgress;

    //construct
    private IDataSaver dataSaver;
    private EnemyProgressVarFactory enemyProgressVarFactory;
    private LevelController levelController;
    private AssetReferenceContainer assetReferenceContainer;
    private AssetProvider assetProvider;

    [Inject]
    public void Construct(
        IDataSaver dataSaver,
        EnemyProgressVarFactory enemyProgressVarFactory,
        LevelController levelController,
        AssetReferenceContainer assetReferenceContainer,
        AssetProvider assetProvider
    ) {

        this.dataSaver = dataSaver;
        this.enemyProgressVarFactory = enemyProgressVarFactory;
        this.levelController = levelController;
        this.assetProvider = assetProvider;
        this.assetReferenceContainer = assetReferenceContainer;

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

        levelController.onPrepareMap += SubscribeOnEnemyDie;
    }

    private void SubscribeOnEnemyDie(List<RoomInfo> roomInfoList) {
        foreach (RoomInfo roomInfo in roomInfoList) {
            foreach (EnemyBase enemy in roomInfo.enemies) {
                enemy.GetComponent<Health>().OnDie += GiveExperience;
            }
        }
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