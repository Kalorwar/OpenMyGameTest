using System;
using UnityEngine;

namespace Project.Scripts.Other
{
    [Serializable]
    public class UnitData
    {
        public ElementType Type;
        public float CellX;
        public float CellY;

        public void Validate(int index, string fileName)
        {
            if (!Enum.IsDefined(typeof(ElementType), Type))
            {
                Debug.LogError($"[JsonLevelParser] ERROR in file '{fileName}': " +
                               $"Unit at index {index} has an invalid type ({(int)Type}). " +
                               "Falling back to default: Fire.");

                Type = ElementType.Fire;
            }
        }
    }
}