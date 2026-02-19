using System;
using System.Collections.Generic;
using Project.Scripts.Other;
using UnityEngine;

namespace Project.Scripts.Level
{
    [Serializable]
    public class LevelData
    {
        private const int DefaultWidth = 6;
        private const int DefaultHeight = 7;

        public int Width;
        public int Height;
        public List<UnitEntry> Units;

        public void ValidateLevel(string fileName)
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
                    Units[i].Validate(i, fileName);
                }
            }
        }
    }
}