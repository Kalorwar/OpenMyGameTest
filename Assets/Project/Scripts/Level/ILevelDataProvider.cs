using Project.Scripts.Datas;

namespace Project.Scripts.Level
{
    public interface ILevelDataProvider
    {
        public LevelData CurrentLevelData { get; }
    }
}