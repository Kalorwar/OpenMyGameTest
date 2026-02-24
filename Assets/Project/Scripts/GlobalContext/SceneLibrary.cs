namespace Project.Scripts.GlobalContext
{
    public static class SceneLibrary
    {
        public const string LevelName = "Level";

        public static string GetLevelNameByLevelId(int levelId)
        {
            return $"Level_{levelId}";
        }
    }
}