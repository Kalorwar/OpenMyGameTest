using System.Collections.Generic;
using Project.Scripts.Grid;
using Project.Scripts.Other;
using Project.Scripts.Units;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Level
{
    public class UnitSpawner : MonoBehaviour
    {
        private const float BaseCellSize = 0.8f;
        private const float BaseUnitScale = 1f;

        [Header("Prefabs")] [SerializeField] private Unit _fireUnitPrefab;

        [SerializeField] private Unit _waterUnitPrefab;
        [SerializeField] private Transform _unitsParent;
        [SerializeField] private LevelGridController _gridController;
        private ILevelDataProvider _levelDataProvider;
        private List<Unit> _spawnedUnits = new();

        [Inject]
        private void Construct(ILevelDataProvider levelDataProvider)
        {
            _levelDataProvider = levelDataProvider;
        }

        private void Start()
        {
            SpawnUnits();
        }

        private void SpawnUnits()
        {
            if (_levelDataProvider.CurrentLevelData.Units == null ||
                _levelDataProvider.CurrentLevelData.Units.Count == 0)
            {
                Debug.LogWarning("[UnitSpawner] No units to spawn");
                return;
            }

            var scaleMultiplier = _gridController.CellSize / BaseCellSize;
            var units = new List<Unit>();

            foreach (var unitData in _levelDataProvider.CurrentLevelData.Units)
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
                var unitData = _levelDataProvider.CurrentLevelData.Units[i];
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