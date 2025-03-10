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

        [SerializeField] private Parameter[] parameters;

        public bool haveFreePoints => pointsLeft > 0;

        private int totalPoints;
        private int pointsLeft;
        private int usedPoints;

        private PlayerParameterLevels parameterLevels;

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

            List<ParameterInfo> parameterInfoList = new List<ParameterInfo>() {
            new ParameterInfo(parameterLevels.moveSpeed, config.moveSpeedProgress, PlayerParameterNames.moveSpeedLocalizedString),
            new ParameterInfo(parameterLevels.rotationSpeed, config.rotationSpeedProgress, PlayerParameterNames.rotationSpeedLocalizedString),

            new ParameterInfo(parameterLevels.dodgeDistance, config.dodgeDistanceProgress, PlayerParameterNames.dodgeDistanceLocalizedString),
            new ParameterInfo(parameterLevels.dodgeDuration, config.dodgeDurationProgress, PlayerParameterNames.dodgeDurationLocalizedString),
            new ParameterInfo(parameterLevels.dodgeInvincibility, config.dodgeInvincibilityProgress, PlayerParameterNames.dodgeInvincibilityLocalizedString),
            new ParameterInfo(parameterLevels.dodgeCooldown, config.dodgeCooldownProgress, PlayerParameterNames.dodgeCooldownLocalizedString),

            new ParameterInfo(parameterLevels.attackRange, config.attackRangeProgress, PlayerParameterNames.attackRangeLocalizedString),
            new ParameterInfo(parameterLevels.attackSpeed, config.attackSpeedProgress, PlayerParameterNames.attackSpeedLocalizedString),
            new ParameterInfo(parameterLevels.attackDamage, config.attackDamageProgress, PlayerParameterNames.attackDamageLocalizedString),

            new ParameterInfo(parameterLevels.maxHealth, config.maxHealthProgress, PlayerParameterNames.maxHealthLocalizedString),
        };

            totalPoints = dataSaver.GetPlayerLevel();

            usedPoints = 0;
            for (int i = 0; i < parameterInfoList.Count; i++) {
                parameters[i].SetInfo(parameterInfoList[i], this);
                usedPoints += parameters[i].GetLevel();
            }

            pointsLeft = totalPoints - usedPoints;

            SetLocalizedStringText();
            UpdateParametersUI();

            LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;

        }

        private void OnLocaleChanged(Locale obj) {
            SetLocalizedStringText();
        }

        private void SetLocalizedStringText() {
            totalPointsText.SetText($"{totalPointsLocalizedString.GetLocalizedString()} {totalPoints}");
            pointsLeftText.SetText($"{pointsLeftLocalizedString.GetLocalizedString()} {pointsLeft}");

        }

        private void UpdateParametersUI() {
            for (int i = 0; i < parameters.Length; i++) {
                parameters[i].UpdateUI();
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

        private void SaveParameters() {
            parameterLevels = new PlayerParameterLevels() {
                moveSpeed = parameters[0].GetLevel(),
                rotationSpeed = parameters[1].GetLevel(),

                dodgeDistance = parameters[2].GetLevel(),
                dodgeDuration = parameters[3].GetLevel(),
                dodgeInvincibility = parameters[4].GetLevel(),
                dodgeCooldown = parameters[5].GetLevel(),

                attackRange = parameters[6].GetLevel(),
                attackSpeed = parameters[7].GetLevel(),
                attackDamage = parameters[8].GetLevel(),

                maxHealth = parameters[9].GetLevel(),
            };
            dataSaver.SavePlayerParameterLevels(parameterLevels);
        }
    }
}