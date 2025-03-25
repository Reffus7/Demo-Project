using UnityEngine;
using System.Collections.Generic;

namespace Project.Map {
    public class RoomInfo {
        public bool isActivated;
        public bool isCleared;
        public Vector3 roomPosition;
        public Vector2 size;
        public Transform roomParent;
        public List<Vector3> enemyPositions;
        public List<Portal> portals;
        public List<Light> lights;

    }

}