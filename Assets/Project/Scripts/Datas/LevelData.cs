using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Datas
{
    [Serializable]
    public class LevelData
    {
        private const int DefaultWidth = 6;
        private const int DefaultHeight = 7;

        public int Width;
        public int Height;
        public List<UnitData> Units;

        // Добавляем опциональный список валидных типов
        public void ValidateLevel(string fileName, List<string> validUnitTypes = null)
        {
            if (Width <= 0 || Height <= 0)
            {
                Debug.LogError($"[JsonLevelParser] ERROR: Invalid grid size ({Width}x{Height}) in '{fileName}'. " +
                               $"Resetting to {DefaultWidth}x{DefaultHeight}.");

                Width = DefaultWidth;
                Height = DefaultHeight;
            }

            if (Units != null)
            {
                for (var i = 0; i < Units.Count; i++)
                {
                    Units[i].Validate(i, validUnitTypes);
                }
            }
        }
    }
}