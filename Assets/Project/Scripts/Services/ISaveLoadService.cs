using Project.Scripts.Datas;

namespace Project.Scripts.Services
{
    public interface ISaveLoadService
    {
        public int CurrentLevelId { get; }
        public void IncreaseCurrentLevel();
        public void Save(LevelData levelData);
        public LevelData LoadLevel();
        public void ClearSavedLevel();
        public void SetCurrentLevel(int currentLevelId);
    }
}