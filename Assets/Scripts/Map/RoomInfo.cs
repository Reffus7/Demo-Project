using UnityEngine;
using System.Collections.Generic;
using Project.Enemy;

namespace Project.Map {
    public class RoomInfo {
        public bool isActivated;
        public bool isCleared;
        public Vector3 roomPosition;
        public Vector2 size;
        public Transform roomParent;
        public List<EnemyBase> enemies; // лучше хранить количество врагов,
                                        // а сами враги будут создаваться через фабрику
                                        // в roomcontroller и будут браться из пула
                                        // (тогда всего врагов будет создано 5)
        public List<Vector3> enemyPositions;
        public List<Portal> portals;
        public List<Light> lights;

    }

}