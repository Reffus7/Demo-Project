using Project.Config;
using Project.HealthSpace;
using Project.Player;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.UI {

    public class PauseMenu : MonoBehaviour {
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button cancelButton;

        [SerializeField] private GameObject root;

        private bool isMenuOpen;

        private IInputHandler inputHandler;
        private SceneLoader sceneLoader;
        private AssetReferenceContainer assetReferenceContainer;

        [Inject]
        public void Construct(
            PlayerController player, 
            IInputHandler inputHandler, 
            SceneLoader sceneLoader,
            AssetReferenceContainer assetReferenceContainer
            
        ) {
            this.inputHandler = inputHandler;
            this.sceneLoader = sceneLoader;
            this.assetReferenceContainer = assetReferenceContainer;

            player.GetComponent<Health>().OnDie += PauseGame;
            inputHandler.onPause += PauseGame;
        }

        private void OnDestroy() {
            inputHandler.onPause -= PauseGame;

        }

        private void PauseGame() {
            isMenuOpen = !isMenuOpen;
            root.SetActive(isMenuOpen ? true : false);
            Time.timeScale = isMenuOpen ? 0 : 1;

        }

        private void Start() {
            Time.timeScale = 1;
            mainMenuButton.onClick.AddListener(GoToMainMenu);
            restartButton.onClick.AddListener(RestartLevel);
            cancelButton.onClick.AddListener(HideExitMenu);
        }

        private void GoToMainMenu() {
            sceneLoader.LoadSceneAsync(assetReferenceContainer.mainMenuScene).Forget();
        }

        private void RestartLevel() {
            sceneLoader.LoadSceneAsync(assetReferenceContainer.gameScene).Forget();
        }

        private void HideExitMenu() {
            Time.timeScale = 1;
            root.SetActive(false);
        }

    }
}