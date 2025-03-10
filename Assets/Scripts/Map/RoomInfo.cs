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
        public List<EnemyBase> enemies;
        public List<Portal> portals;
        public List<Light> lights;

    }

}