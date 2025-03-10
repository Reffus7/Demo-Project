using System.IO;
using UnityEditor;
using UnityEngine;

namespace Project.Data {

    public class DataSaver : IDataSaver {

#if UNITY_EDITOR
        [MenuItem("Project/Clear all saved data")]
        private static void ClearData() {
            PlayerPrefs.DeleteAll();
            string json = JsonUtility.ToJson(new PlayerParameterLevels());
            File.WriteAllText(playerParametersFilePath, json);
        }
#endif

        public int GetGameLevel() {
            return PlayerPrefs.GetInt(DataKeys.GameLevel, 0);
        }

        public void SaveGameLevel(int level) {
            PlayerPrefs.SetInt(DataKeys.GameLevel, level);
        }

        public int GetPlayerLevel() {
            return PlayerPrefs.GetInt(DataKeys.PlayerLevel, 0);
        }

        public void SavePlayerLevel(int level) {
            PlayerPrefs.SetInt(DataKeys.PlayerLevel, level);

        }

        public int GetPlayerExperience() {
            return PlayerPrefs.GetInt(DataKeys.PlayerExperience, 0);
        }

        public void SavePlayerExperience(int experience) {
            PlayerPrefs.SetInt(DataKeys.PlayerExperience, experience);

        }

        private static string playerParametersFilePath => Path.Combine(Application.persistentDataPath, "player_parameters.json");

        public PlayerParameterLevels GetPlayerParameterLevels() {
            if (File.Exists(playerParametersFilePath)) {
                string json = File.ReadAllText(playerParametersFilePath);
                return JsonUtility.FromJson<PlayerParameterLevels>(json);
            }
            return new PlayerParameterLevels();
        }

        public void SavePlayerParameterLevels(PlayerParameterLevels playerParameterLevels) {
            string json = JsonUtility.ToJson(playerParameterLevels, true);
            File.WriteAllText(playerParametersFilePath, json);
        }


    }
}