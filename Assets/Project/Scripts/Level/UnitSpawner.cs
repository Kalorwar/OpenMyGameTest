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

        private DiContainer _container;
        private LevelGridController _gridController;
        private ILevelDataProvider _levelDataProvider;
        private List<Unit> _spawnedUnits = new();

        [Inject]
        private void Construct(ILevelDataProvider levelDataProvider, LevelGridController gridController,
            DiContainer container)
        {
            _levelDataProvider = levelDataProvider;
            _gridController = gridController;
            _container = container;
        }

        private void Start()
        {
            SpawnUnits();
        }

        private void SpawnUnits()
        {
            var levelData = _levelDataProvider.CurrentLevelData;

            if (!HasValidUnitData(levelData))
            {
                Debug.LogWarning("[UnitSpawner] No units to spawn");
                return;
            }

            var scaleMultiplier = CalculateScaleMultiplier();
            var units = CreateUnits(levelData.Units, scaleMultiplier);
            PlaceUnitsOnGrid(units, levelData.Units);

            _spawnedUnits = units;
        }

        private static bool HasValidUnitData(LevelData levelData)
        {
            return levelData.Units != null && levelData.Units.Count > 0;
        }

        private float CalculateScaleMultiplier()
        {
            return _gridController.GetCellSize() / BaseCellSize;
        }

        private List<Unit> CreateUnits(List<UnitData> unitEntries, float scaleMultiplier)
        {
            var units = new List<Unit>(unitEntries.Count);

            foreach (var entry in unitEntries)
            {
                var unit = TryCreateUnit(entry, scaleMultiplier);
                if (unit != null)
                {
                    units.Add(unit);
                }
            }

            return units;
        }

        private Unit TryCreateUnit(UnitData entry, float scaleMultiplier)
        {
            var prefab = GetPrefab(entry.Type);

            if (prefab == null)
            {
                Debug.LogError($"[UnitSpawner] No prefab for unit type: {entry.Type}");
                return null;
            }

            var unit = _container.InstantiatePrefabForComponent<Unit>(prefab, Vector3.zero, Quaternion.identity,
                _unitsParent);

            ApplyScale(unit, scaleMultiplier);

            return unit;
        }

        private void ApplyScale(Unit unit, float scaleMultiplier)
        {
            var scale = BaseUnitScale * scaleMultiplier;
            unit.transform.localScale = new Vector3(scale, scale, 1f);
        }

        private void PlaceUnitsOnGrid(List<Unit> units, List<UnitData> unitEntries)
        {
            for (var i = 0; i < units.Count; i++)
            {
                var x = (int)unitEntries[i].CellX;
                var y = (int)unitEntries[i].CellY;

                _gridController.PlaceUnitAtCell(units[i], x, y);
            }
        }

        private Unit GetPrefab(ElementType type)
        {
            return type == ElementType.Fire ? _fireUnitPrefab : _waterUnitPrefab;
        }
    }
}