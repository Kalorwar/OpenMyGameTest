using System.Collections.Generic;
using System.IO;
using Project.Scripts.Datas;
using UnityEngine;

namespace Project.Scripts.Services
{
    public class JsonLevelLoader
    {
        public LevelData LoadLevel(string fileName, List<string> validUnitTypes = null)
        {
            string jsonText;
            string source;

            var persistentPath = Path.Combine(Application.persistentDataPath, fileName);
            if (File.Exists(persistentPath))
            {
                jsonText = File.ReadAllText(persistentPath);
                source = $"PersistentDataPath ({persistentPath})";
            }
            else
            {
                var asset = Resources.Load<TextAsset>(fileName);
                if (asset == null)
                {
                    Debug.LogError(
                        $"[JsonLevelParser] ERROR: File '{fileName}' not found in Resources or PersistentDataPath!");
                    return null;
                }

                jsonText = asset.text;
                source = $"Resources ({fileName})";
            }

            var data = JsonUtility.FromJson<LevelData>(jsonText);

            if (data != null)
            {
                data.ValidateLevel(fileName, validUnitTypes);
            }
            else
            {
                Debug.LogError($"[JsonLevelParser] ERROR: Failed to parse '{fileName}' from {source}.");
            }

            return data;
        }
    }
}