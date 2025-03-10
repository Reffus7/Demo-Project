using Project.Progress;
using UnityEngine.Localization;

namespace Project.UI {

    public class ParameterInfo {
        public int level;
        public PlayerProgressVar progress;
        public LocalizedString localizedString;

        public ParameterInfo(int level, PlayerProgressVar progress, LocalizedString localizedString) {
            this.level = level;
            this.progress = progress;
            this.localizedString = localizedString;
        }
    }
}