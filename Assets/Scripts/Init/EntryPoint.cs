using Cysharp.Threading.Tasks;
using Project.Config;
using Project.Factory;
using Project.Player;
using Project.UI.Mobile;
using UnityEngine;
using Zenject;

namespace Project.Init {
    public class EntryPoint : MonoBehaviour {
        private PlayerController player;

        private DiContainer diContainer;
        private LevelController levelController;
        private AssetProvider assetProvider;
        private AssetReferenceContainer assetReferenceContainer;
        private ZenjectInstantiator zenjectInstantiator;
        private Canvas canvas;
        private PlayerFactory playerFactory;

        [Inject]
        public void Construct(
            DiContainer diContainer,
            LevelController levelController,
            AssetProvider assetProvider,
            AssetReferenceContainer assetReferenceContainer,
            ZenjectInstantiator zenjectInstantiator,
            Canvas canvas,
            PlayerFactory playerFactory
        ) {

            this.diContainer = diContainer;
            this.levelController = levelController;
            this.assetProvider = assetProvider;
            this.assetReferenceContainer = assetReferenceContainer;
            this.zenjectInstantiator = zenjectInstantiator;
            this.canvas = canvas;
            this.playerFactory = playerFactory;

        }

        private void Start() {
            InitAsync().Forget();
        }

        private async UniTaskVoid InitAsync() {

            LoadingScreen.instance.Show(true);

#if UNITY_ANDROID
            await InitMobile();
#endif

            await InitPlayer();
            await InitUI();

            levelController.StartLevel(player);

            LoadingScreen.instance.Hide(true);
        }

        private async UniTask InitMobile() {
            GameObject mobileCanvasPrefab = await assetProvider.LoadAssetAsync<GameObject>(assetReferenceContainer.mobileCanvas);
            MobileCanvas mobileCanvas = zenjectInstantiator.Instantiate(mobileCanvasPrefab).GetComponent<MobileCanvas>();

            diContainer.Bind<MobileCanvas>().FromInstance(mobileCanvas).AsSingle();
        }

        private async UniTask InitPlayer() {
            player = await playerFactory.Create();

            diContainer.Bind<PlayerController>().FromInstance(player).AsSingle();
        }

        private async UniTask InitUI() {
            GameObject playerUI = await assetProvider.LoadAssetAsync<GameObject>(assetReferenceContainer.playerUI);
            zenjectInstantiator.Instantiate(playerUI, canvas.transform);

#if UNITY_ANDROID
            diContainer.BindInterfacesAndSelfTo<UnityAdsManager>().AsSingle().NonLazy();

            GameObject adsMenu = await assetProvider.LoadAssetAsync<GameObject>(assetReferenceContainer.adsMenuCanvas);
            zenjectInstantiator.Instantiate(adsMenu);
#endif
        }

    }

}