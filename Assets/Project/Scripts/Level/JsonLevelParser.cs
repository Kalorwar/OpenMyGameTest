using UnityEngine;

namespace Project.Scripts.Level
{
    public static class JsonLevelParser
    {
        public static LevelData LoadLevel(string fileName)
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
                data.ValidateLevel(fileName);
            }
            else
            {
                Debug.LogError($"[JsonLevelParser] ERROR: Failed to parse '{fileName}'.");
            }

            return data;
        }
    }
}