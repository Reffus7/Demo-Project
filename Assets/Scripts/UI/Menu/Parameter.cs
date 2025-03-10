using Project.Progress;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace Project.UI {

    public class Parameter : MonoBehaviour {
        [SerializeField] private LocalizeStringEvent nameLocalizedString;
        [SerializeField] private TextMeshProUGUI levelNumberText;
        [SerializeField] private TextMeshProUGUI valueNumberText;
        [SerializeField] private Button minusButton;
        [SerializeField] private Button plusButton;

        private int maxLevel = 5;

        private float value => valueProgress.Value;

        private int level;
        private PlayerProgressVar valueProgress;
        private ParameterController parameterController;

        private void Start() {
            plusButton.onClick.AddListener(LevelUp);
            minusButton.onClick.AddListener(LevelDown);

        }

        public void SetInfo(ParameterInfo info, ParameterController parameterController) {
            nameLocalizedString.StringReference = info.localizedString;
            level = info.level;
            valueProgress = info.progress;
            this.parameterController = parameterController;

            UpdateUI();
        }

        public int GetLevel() {
            return level;
        }

        private void LevelUp() {
            level++;
            parameterController.DecreaseFreePoints();
            UpdateUI();
        }

        private void LevelDown() {
            level--;
            parameterController.IncreaseFreePoints();
            UpdateUI();

        }

        public void UpdateUI() {

            valueProgress.SetLevel(level);

            levelNumberText.SetText(level.ToString());
            valueNumberText.SetText(value.ToString());

            SetButtonsInteracable();

        }

        private void SetButtonsInteracable() {
            if (level == 0) {
                minusButton.interactable = false;
            }
            else {
                minusButton.interactable = true;
            }

            if (level == maxLevel || !parameterController.haveFreePoints) {
                plusButton.interactable = false;
            }
            else {
                plusButton.interactable = true;
            }
        }

    }
}