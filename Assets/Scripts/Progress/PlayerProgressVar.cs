using System;

namespace Project.Progress {

    [Serializable]
    public class PlayerProgressVar : ProgressVar {

        public void SetLevel(int level) {
            this.level = level;
        }


    }
}