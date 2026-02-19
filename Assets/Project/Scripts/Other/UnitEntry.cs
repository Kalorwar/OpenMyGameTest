using System;
using UnityEngine;

namespace Project.Scripts.Other
{
    [Serializable]
    public class UnitEntry
    {
        public ElementType type;
        public float cellX;
        public float cellY;

        public void Validate(int index, string fileName)
        {
            if (!Enum.IsDefined(typeof(ElementType), type))
            {
                Debug.LogError($"[JsonLevelParser] ERROR in file '{fileName}': " +
                               $"Unit at index {index} has an invalid type ({(int)type}). " +
                               "Falling back to default: Fire.");

                type = ElementType.Fire;
            }
        }
    }
}