using Project.Scripts.Datas;
using Project.Scripts.GlobalContext;
using UnityEngine;

namespace Project.Scripts.Services
{
    public class SaveLoadService : ISaveLoadService
    {
        private const string CurrentLevelSaveKey = "CurrentLevel";
        private readonly JsonLevelLoader _jsonLevelLoader = new();
        private readonly JsonLevelSaver _jsonLevelSaver = new();

        public SaveLoadService()
        {
            LoadCurrentLevelId();
        }

        public int CurrentLevelId { get; private set; }

        public void IncreaseCurrentLevel()
        {
            var nextLevelId = CurrentLevelId + 1;

            if (LevelExists(nextLevelId))
            {
                CurrentLevelId = nextLevelId;
            }
            else
            {
                CurrentLevelId = 1;
            }

            SaveCurrentLevelId();
        }

        public void Save(LevelData levelData)
        {
            _jsonLevelSaver.SaveLevel(levelData, GetFileName());
            SaveCurrentLevelId();
        }

        public LevelData LoadLevel()
        {
            return _jsonLevelLoader.LoadLevel(GetFileName());
        }

        public void ClearSavedLevel()
        {
            _jsonLevelSaver.DeleteLevel(GetFileName());
        }

        public void SetCurrentLevel(int currentLevelId)
        {
            CurrentLevelId = currentLevelId;
            SaveCurrentLevelId();
        }

        private bool LevelExists(int levelId)
        {
            var levelName = SceneLibrary.GetLevelNameByLevelId(levelId);
            var textAsset = Resources.Load<TextAsset>(levelName);
            var exists = textAsset != null;

            if (textAsset != null)
            {
                Resources.UnloadAsset(textAsset);
            }

            return exists;
        }

        private void LoadCurrentLevelId()
        {
            CurrentLevelId = PlayerPrefs.GetInt(CurrentLevelSaveKey, 1);
            if (!LevelExists(CurrentLevelId))
            {
                CurrentLevelId = 1;
                SaveCurrentLevelId();
            }
        }

        private void SaveCurrentLevelId()
        {
            PlayerPrefs.SetInt(CurrentLevelSaveKey, CurrentLevelId);
            PlayerPrefs.Save();
        }

        private string GetFileName()
        {
            return $"{SceneLibrary.GetLevelNameByLevelId(CurrentLevelId)}";
        }
    }
}