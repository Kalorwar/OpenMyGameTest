using Project.Scripts.Datas;
using Project.Scripts.GlobalContext;

namespace Project.Scripts.Level
{
    public class LevelDataProvider : ILevelDataProvider
    {
        public LevelData CurrentLevelData { get; } = JsonLevelParser.LoadLevel(SceneLibrary.GetLevelNameByLevelId(2));
    }
}