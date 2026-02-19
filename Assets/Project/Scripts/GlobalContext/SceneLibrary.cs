using System.Collections.Generic;
using System.Linq;

namespace Project.Scripts.GlobalContext
{
    public static class SceneLibrary
    {
        public const string LevelName = "Level";
        private const int MaxLevelId = 3;
        private const int FirstsLevelId = 1;

        private static readonly Dictionary<int, string> LevelIdToSceneMap =
            Enumerable.Range(FirstsLevelId, MaxLevelId).ToDictionary(a => a, a => $"Level_{a}");

        public static string GetLevelNameByLevelId(int levelId)
        {
            return LevelIdToSceneMap.GetValueOrDefault(levelId, LevelIdToSceneMap[FirstsLevelId]);
        }
    }
}