using Project.Config;
using UnityEngine;
using Zenject;

namespace Project.Init {

    public class BootScene : MonoBehaviour {
        [SerializeField] private LoadingScreen loadingScreenPrefab;

        private AssetReferenceContainer assetReferenceContainer;
        private SceneLoader sceneLoader;

        [Inject]
        public void Construct(
            AssetReferenceContainer assetReferenceContainer,
            SceneLoader sceneLoader

        ) {
            this.assetReferenceContainer = assetReferenceContainer;
            this.sceneLoader = sceneLoader;
        }

        private void Start() {
            Application.targetFrameRate = 250;

            sceneLoader.LoadSceneAsync(assetReferenceContainer.mainMenuScene).Forget();
        }
    }
}