using Project.Scripts.Datas;

namespace Project.Scripts.Services
{
    public class LevelDataProvider : ILevelDataProvider
    {
        public LevelDataProvider(ISaveLoadService saveLoadService)
        {
            CurrentLevelData = saveLoadService.LoadLevel();
        }

        public LevelData CurrentLevelData { get; }
    }
}