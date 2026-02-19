using System.Collections.Generic;
using Project.Scripts.Other;
using Project.Scripts.Units;
using UnityEngine;

namespace Project.Scripts.Level
{
    public class UnitSpawner : MonoBehaviour
    {
        private const float BaseCellSize = 0.8f;
        private const float BaseUnitScale = 1f;

        [Header("Grid")] [SerializeField] private LevelGridController _gridController;

        [Header("Prefabs")] [SerializeField] private Unit _fireUnitPrefab;

        [SerializeField] private Unit _waterUnitPrefab;

        [Header("Level")] [SerializeField] private string _levelFileName = "Level_1";

        [SerializeField] private Transform _unitsParent;

        private LevelData _currentLevelData;
        private List<Unit> _spawnedUnits = new();

        private void Start()
        {
            SpawnLevel();
        }

        public void SpawnLevel()
        {
            _currentLevelData = JsonLevelParser.LoadLevel(_levelFileName);

            if (_currentLevelData == null)
            {
                Debug.LogError($"[UnitSpawner] Failed to load level: {_levelFileName}");
                return;
            }

            _gridController.Initialize(_currentLevelData.Width, _currentLevelData.Height);

            SpawnUnits(_currentLevelData);
        }

        private void SpawnUnits(LevelData levelData)
        {
            if (levelData.Units == null || levelData.Units.Count == 0)
            {
                Debug.LogWarning("[UnitSpawner] No units to spawn");
                return;
            }

            var scaleMultiplier = _gridController.CellSize / BaseCellSize;
            var units = new List<Unit>();

            foreach (var unitData in levelData.Units)
            {
                var prefab = GetPrefab(unitData.Type);

                if (prefab == null)
                {
                    Debug.LogError($"[UnitSpawner] No prefab for unit type: {unitData.Type}");
                    continue;
                }

                var unit = Instantiate(prefab, Vector3.zero, Quaternion.identity, _unitsParent);

                var sortOrder = (int)unitData.CellY * 10 + (int)unitData.CellX;
                unit.Initialize(sortOrder);

                var unitScale = BaseUnitScale * scaleMultiplier;
                unit.transform.localScale = new Vector3(unitScale, unitScale, 1);

                units.Add(unit);
            }

            for (var i = 0; i < units.Count; i++)
            {
                var unitData = levelData.Units[i];
                var x = (int)unitData.CellX;
                var y = (int)unitData.CellY;

                _gridController.PlaceUnitAtCell(units[i], x, y);
            }

            _spawnedUnits = units;
        }

        private Unit GetPrefab(ElementType type)
        {
            return type == ElementType.Fire ? _fireUnitPrefab : _waterUnitPrefab;
        }
    }
}