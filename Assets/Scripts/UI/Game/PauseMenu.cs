using Cysharp.Threading.Tasks;
using Project.Config;
using Project.HealthSpace;
using Project.Input;
using Project.Player;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.UI {

    public class PauseMenu : MonoBehaviour {
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button cancelButton;

        [SerializeField] private GameObject root;

        private bool isMenuOpen => root.activeSelf;

        private IInputHandler inputHandler;
        private SceneLoader sceneLoader;
        private AssetReferenceContainer assetReferenceContainer;
        private CancellationToken cancellationToken;

        [Inject]
        public void Construct(
            PlayerController player,
            IInputHandler inputHandler,
            SceneLoader sceneLoader,
            AssetReferenceContainer assetReferenceContainer,
            CancellationToken cancellationToken

        ) {
            this.inputHandler = inputHandler;
            this.sceneLoader = sceneLoader;
            this.assetReferenceContainer = assetReferenceContainer;
            this.cancellationToken = cancellationToken;

            player.GetComponent<Health>().OnDie += () => PauseGameWithDelay().Forget();
            inputHandler.onPause += PauseGame;
        }

        private void OnDestroy() {
            if (inputHandler != null) inputHandler.onPause -= PauseGame;

        }

        private const int dieAnimationDurationMiliseconds = 2669;

        private bool startPauseGame = false;

        private async UniTaskVoid PauseGameWithDelay() {
            if (startPauseGame) return;

            startPauseGame = true;

            await UniTask.Delay(dieAnimationDurationMiliseconds, cancellationToken: cancellationToken);

            PauseGame();

            startPauseGame = false;
        }

        private void PauseGame() {
            Time.timeScale = isMenuOpen ? 1 : 0;
            root.SetActive(isMenuOpen ? false : true);

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