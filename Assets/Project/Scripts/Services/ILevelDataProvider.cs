using Project.Scripts.Datas;

namespace Project.Scripts.Services
{
    public interface ILevelDataProvider
    {
        public LevelData CurrentLevelData { get; }
    }
}