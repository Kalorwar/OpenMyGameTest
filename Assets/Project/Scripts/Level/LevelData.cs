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

        public int width;
        public int height;
        public List<UnitEntry> units;

        public void ValidateLevel(string fileName)
        {
            if (width <= 0 || height <= 0)
            {
                Debug.LogError($"[JsonLevelParser] ERROR: Invalid grid size ({width}x{height}) in '{fileName}'. " +
                               $"Resetting to {DefaultWidth}x{DefaultHeight}.");

                width = DefaultWidth;
                height = DefaultHeight;
            }

            if (units != null)
            {
                for (var i = 0; i < units.Count; i++)
                {
                    units[i].Validate(i, fileName);
                }
            }
        }
    }
}