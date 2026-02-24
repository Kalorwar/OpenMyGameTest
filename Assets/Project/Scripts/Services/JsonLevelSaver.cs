using System;
using System.IO;
using Project.Scripts.Datas;
using UnityEngine;

namespace Project.Scripts.Services
{
    public class JsonLevelSaver
    {
        public void SaveLevel(LevelData data, string fileName)
        {
            if (data == null)
            {
                Debug.LogError("[JsonLevelSaver] ERROR: Cannot save null LevelData!");
                return;
            }

            try
            {
                var json = JsonUtility.ToJson(data, true);

                var filePath = Path.Combine(Application.persistentDataPath, fileName);

                var directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.WriteAllText(filePath, json);
            }
            catch (Exception e)
            {
                Debug.LogError($"[JsonLevelSaver] ERROR: Failed to save level '{fileName}': {e.Message}");
            }
        }

        public void DeleteLevel(string fileName)
        {
            var filePath = GetFilePath(fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private string GetFilePath(string fileName)
        {
            return Path.Combine(Application.persistentDataPath, fileName);
        }
    }
}