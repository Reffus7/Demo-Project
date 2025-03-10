using Cysharp.Threading.Tasks;
using Project.Config;
using Project.Player;
using UnityEngine;
using Zenject;

namespace Project.Factory {
    public class PlayerFactory : Factory, IInitializable {
        private PlayerConfig config;
        private GameObject playerPrefab;

        private AssetReferenceContainer assetReferenceContainer;
        private AssetProvider assetProvider;

        [Inject]
        public void Construct(AssetReferenceContainer assetReferenceContainer, AssetProvider assetProvider) {
            this.assetReferenceContainer = assetReferenceContainer;
            this.assetProvider = assetProvider;
        }

        public void Initialize() {
            InitAsync().Forget();
        }

        private async UniTaskVoid InitAsync() {
            config = await assetProvider.LoadAssetAsync<PlayerConfig>(assetReferenceContainer.playerConfig);

            playerPrefab = await assetProvider.LoadAssetAsync<GameObject>(assetReferenceContainer.player);
        }

        public async UniTask<PlayerController> Create() {
            await UniTask.WaitWhile(() => config == null || playerPrefab == null);

            PlayerController player = zenjectInstantiator.Instantiate(playerPrefab).GetComponent<PlayerController>();

            player.Init(config);

            return player;
        }

    }
}