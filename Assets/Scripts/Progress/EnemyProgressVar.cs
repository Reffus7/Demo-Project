using System;
using Zenject;

namespace Project.Progress {

    [Serializable]
    public class EnemyProgressVar : ProgressVar {
        [Inject]
        public void Construct(LevelController levelController) {
            level = levelController.level;
            levelController.onLevelChanged += UpdateLevel;
        }

        private void UpdateLevel(int level) {
            this.level = level;
        }

    }
}