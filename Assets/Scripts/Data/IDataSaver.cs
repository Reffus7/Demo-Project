namespace Project.Data {
    public interface IDataSaver {

        public int GetGameLevel();
        public void SaveGameLevel(int level);

        public int GetPlayerLevel();
        public void SavePlayerLevel(int level);

        public int GetPlayerExperience();
        public void SavePlayerExperience(int experience);

        public PlayerParameterLevels GetPlayerParameterLevels();
        public void SavePlayerParameterLevels(PlayerParameterLevels playerParameterLevels);
    }
}