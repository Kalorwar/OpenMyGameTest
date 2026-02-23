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
            if (CurrentLevelId < SceneLibrary.MaxLevelId)
            {
                CurrentLevelId += 1;
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

        private void LoadCurrentLevelId()
        {
            CurrentLevelId = PlayerPrefs.GetInt(CurrentLevelSaveKey, 1);
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