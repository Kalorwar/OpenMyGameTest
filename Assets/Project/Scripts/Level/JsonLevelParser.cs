using System.Collections.Generic;
using Project.Scripts.Datas;
using UnityEngine;

namespace Project.Scripts.Level
{
    public static class JsonLevelParser
    {
        public static LevelData LoadLevel(string fileName, List<string> validUnitTypes = null)
        {
            var asset = Resources.Load<TextAsset>(fileName);
            if (asset == null)
            {
                Debug.LogError($"[JsonLevelParser] ERROR: File '{fileName}' not found in Resources!");
                return null;
            }

            var data = JsonUtility.FromJson<LevelData>(asset.text);

            if (data != null)
            {
                data.ValidateLevel(fileName, validUnitTypes);
            }
            else
            {
                Debug.LogError($"[JsonLevelParser] ERROR: Failed to parse '{fileName}'.");
            }

            return data;
        }
    }
}