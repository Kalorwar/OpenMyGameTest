using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Datas
{
    [Serializable]
    public class UnitData
    {
        public string UnitType;
        public float CellX;
        public float CellY;

        public void Validate(int index, List<string> validTypes)
        {
            if (string.IsNullOrEmpty(UnitType))
            {
                Debug.LogError($"[JsonLevelParser] ERROR: Unit at index {index} has empty type!");
                UnitType = validTypes.Count > 0 ? validTypes[0] : "unknown";
            }
        }
    }
}