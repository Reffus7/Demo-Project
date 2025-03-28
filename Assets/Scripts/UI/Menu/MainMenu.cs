using Project.Config;
using Project.Data;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using Zenject;

namespace Project.UI {

    public class MainMenu : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private LocalizedString levelLocalizedString;

        [SerializeField] private Button startGameButton;
        [SerializeField] private Button upgradeMenuButton;
        [SerializeField] private Button exitGameButton;
        [SerializeField] private Button backToMainMenuButton;

        [SerializeField] private CanvasGroup mainMenu;
        [SerializeField] private CanvasGroup upgradeMenu;

        private List<CanvasGroup> menuList;

        private IDataSaver dataSaver;
        private SceneLoader sceneLoader;
        private AssetReferenceContainer assetReferenceContainer;

        [Inject]
        public void Construct(
            IDataSaver dataSaver,
            SceneLoader sceneLoader,
            AssetReferenceContainer assetReferenceContainer

        ) {

            this.dataSaver = dataSaver;
            this.sceneLoader = sceneLoader;
            this.assetReferenceContainer = assetReferenceContainer;
        }

        private void Start() {
            startGameButton.onClick.AddListener(StartLevel);
            upgradeMenuButton.onClick.AddListener(() => OpenMenu(upgradeMenu));
            exitGameButton.onClick.AddListener(ExitGame);
            backToMainMenuButton.onClick.AddListener(() => OpenMenu(mainMenu));

            menuList = new List<CanvasGroup>() { mainMenu, upgradeMenu };

            SetLocalizedStringText();
            LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
        }

        private void OnLocaleChanged(Locale obj) {
            SetLocalizedStringText();
        }

        private void SetLocalizedStringText() {
            levelText.SetText($"{levelLocalizedString.GetLocalizedString()} {dataSaver.GetGameLevel()}");

        }

        private void StartLevel() {
            sceneLoader.LoadSceneAsync(assetReferenceContainer.gameScene).Forget();

        }

        private void OpenMenu(CanvasGroup menu) {
            foreach (CanvasGroup canvasGroup in menuList) {
                canvasGroup.alpha = 0;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
            menu.alpha = 1;
            menu.interactable = true;
            menu.blocksRaycasts = true;
        }

        private void ExitGame() {
            Application.Quit();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif

        }
    }
}