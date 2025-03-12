using Cysharp.Threading.Tasks;
using Project.Data;
using Project.Map;
using Project.Player;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelController : ITickable, IInitializable {

    public event System.Action<int> onLevelChanged;
    public event System.Action<List<RoomInfo>> onPrepareMap;

    public int level { get; private set; }

    private List<RoomInfo> roomInfoList;

    private MapGenerator mapGenerator;
    private PlayerController player;
    private IDataSaver dataSaver;


    [Inject]
    public void Construct(MapGenerator mapGenerator, IDataSaver dataSaver) {
        this.mapGenerator = mapGenerator;
        this.dataSaver = dataSaver;
    }

    public void Tick() {
        if (roomInfoList == null) return;

        if (!roomInfoList.Exists(room => !room.isCleared)) {
            if (isGeneratingMap) return;

            StartNewLevel();
        }
    }

    private bool isGeneratingMap = false;

    private void StartNewLevel() {

        isGeneratingMap = true;

        level++;
        dataSaver.SaveGameLevel(level);
        onLevelChanged?.Invoke(level);

        PrepareMap();

        isGeneratingMap = false;

    }

    public void Initialize() {
        level = dataSaver.GetGameLevel();
        onLevelChanged?.Invoke(level);
    }

    public void StartLevel(PlayerController player) {
        this.player = player;
        PrepareMap();

    }

    private void PrepareMap() {
        mapGenerator.ClearMap();
        mapGenerator.GenerateMap();
        roomInfoList = mapGenerator.GetRoomInfoList();

        RoomInfo spawnRoom = roomInfoList[Random.Range(0, roomInfoList.Count)];
        player.transform.position = spawnRoom.roomPosition;

        onPrepareMap?.Invoke(roomInfoList);
    }


}