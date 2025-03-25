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

    public class ParameterController : MonoBehaviour {
        [SerializeField] private Button backToMainMenuButton;

        [SerializeField] private TextMeshProUGUI totalPointsText;
        [SerializeField] private LocalizedString totalPointsLocalizedString;

        [SerializeField] private TextMeshProUGUI pointsLeftText;
        [SerializeField] private LocalizedString pointsLeftLocalizedString;

        [SerializeField] private Parameter[] uiParameters;

        public bool haveFreePoints => pointsLeft > 0;

        private int totalPoints;
        private int pointsLeft;
        private int usedPoints;

        private PlayerParameterLevels parameterLevels;

        private Dictionary<PlayerParameterType, Parameter> parameterDict;
        private Dictionary<PlayerParameterType, ParameterInfo> parameterInfoDict;

        private PlayerConfig config;
        private IDataSaver dataSaver;

        [Inject]
        public void Construct(PlayerConfig config, IDataSaver dataSaver) {
            this.config = config;
            this.dataSaver = dataSaver;
        }

        private void Start() {
            backToMainMenuButton.onClick.AddListener(SaveParameters);

            parameterLevels = dataSaver.GetPlayerParameterLevels();

            parameterInfoDict = new Dictionary<PlayerParameterType, ParameterInfo> {
                { PlayerParameterType.moveSpeed, new ParameterInfo(GetLevel(PlayerParameterType.moveSpeed), config.moveSpeedProgress, PlayerParameterNames.moveSpeedLocalizedString) },
                { PlayerParameterType.rotationSpeed, new ParameterInfo(GetLevel(PlayerParameterType.rotationSpeed), config.rotationSpeedProgress, PlayerParameterNames.rotationSpeedLocalizedString) },
                { PlayerParameterType.dodgeDistance, new ParameterInfo(GetLevel(PlayerParameterType.dodgeDistance), config.dodgeDistanceProgress, PlayerParameterNames.dodgeDistanceLocalizedString) },
                { PlayerParameterType.dodgeDuration, new ParameterInfo(GetLevel(PlayerParameterType.dodgeDuration), config.dodgeDurationProgress, PlayerParameterNames.dodgeDurationLocalizedString) },
                { PlayerParameterType.dodgeInvincibility, new ParameterInfo(GetLevel(PlayerParameterType.dodgeInvincibility), config.dodgeInvincibilityProgress, PlayerParameterNames.dodgeInvincibilityLocalizedString) },
                { PlayerParameterType.dodgeCooldown, new ParameterInfo(GetLevel(PlayerParameterType.dodgeCooldown), config.dodgeCooldownProgress, PlayerParameterNames.dodgeCooldownLocalizedString) },
                { PlayerParameterType.attackRange, new ParameterInfo(GetLevel(PlayerParameterType.attackRange), config.attackRangeProgress, PlayerParameterNames.attackRangeLocalizedString) },
                { PlayerParameterType.attackSpeed, new ParameterInfo(GetLevel(PlayerParameterType.attackSpeed), config.attackSpeedProgress, PlayerParameterNames.attackSpeedLocalizedString) },
                { PlayerParameterType.attackDamage, new ParameterInfo(GetLevel(PlayerParameterType.attackDamage), config.attackDamageProgress, PlayerParameterNames.attackDamageLocalizedString) },
                { PlayerParameterType.maxHealth, new ParameterInfo(GetLevel(PlayerParameterType.maxHealth), config.maxHealthProgress, PlayerParameterNames.maxHealthLocalizedString) }
            };

            parameterDict = new Dictionary<PlayerParameterType, Parameter>();
            for (int i = 0; i < uiParameters.Length; i++) {
                parameterDict[(PlayerParameterType)i] = uiParameters[i];
            }

            totalPoints = dataSaver.GetPlayerLevel();

            usedPoints = 0;

            foreach (var kvp in parameterInfoDict) {
                if (parameterDict.TryGetValue(kvp.Key, out var parameter)) {
                    parameter.SetInfo(kvp.Value, this);
                    usedPoints += parameter.GetLevel();
                }
            }

            pointsLeft = totalPoints - usedPoints;

            SetLocalizedStringText();
            UpdateParametersUI();

            LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;

        }

        private int GetLevel(PlayerParameterType type) {
            return parameterLevels.GetLevel(type);
        }

        private void SaveParameters() {
            foreach (var kvp in parameterDict) {
                parameterLevels.Levels[kvp.Key] = kvp.Value.GetLevel();
            }

            dataSaver.SavePlayerParameterLevels(parameterLevels);
        }

        private void OnLocaleChanged(Locale obj) {
            SetLocalizedStringText();
        }

        private void SetLocalizedStringText() {
            totalPointsText.SetText($"{totalPointsLocalizedString.GetLocalizedString()} {totalPoints}");
            pointsLeftText.SetText($"{pointsLeftLocalizedString.GetLocalizedString()} {pointsLeft}");

        }

        private void UpdateParametersUI() {
            for (int i = 0; i < uiParameters.Length; i++) {
                uiParameters[i].UpdateUI();
            }
        }

        public void IncreaseFreePoints() {
            pointsLeft++;
            UpdatePointsUI();
            UpdateParametersUI();
        }

        public void DecreaseFreePoints() {
            pointsLeft--;
            UpdatePointsUI();
            UpdateParametersUI();

        }
        public void UpdatePointsUI() {
            SetLocalizedStringText();
        }

    }
}