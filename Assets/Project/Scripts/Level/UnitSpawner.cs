using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Datas;
using Project.Scripts.Grid;
using Project.Scripts.ScriptableObjects;
using Project.Scripts.Services;
using Project.Scripts.Units;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Level
{
    public class UnitSpawner : MonoBehaviour
    {
        private const float BaseCellSize = 0.8f;
        private const float BaseUnitScale = 1f;

        [SerializeField] private Unit _baseUnitPrefab;

        private DiContainer _container;
        private LevelGridController _gridController;
        private ILevelDataProvider _levelDataProvider;
        private UnitConfigurationSo _unitConfigurationSo;

        [Inject]
        private void Construct(ILevelDataProvider levelDataProvider, LevelGridController gridController,
            DiContainer container, UnitConfigurationSo unitConfigurationSo)
        {
            _levelDataProvider = levelDataProvider;
            _gridController = gridController;
            _container = container;
            _unitConfigurationSo = unitConfigurationSo;
        }

        public List<Unit> SpawnedUnits { get; private set; } = new();

        public void SpawnUnits()
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

            SpawnedUnits = units;
        }

        private static bool HasValidUnitData(LevelData levelData)
        {
            return levelData.Units != null && levelData.Units.Length > 0;
        }

        private float CalculateScaleMultiplier()
        {
            return _gridController.GetCellSize() / BaseCellSize;
        }

        private List<Unit> CreateUnits(UnitData[] unitEntries, float scaleMultiplier)
        {
            var units = new List<Unit>(unitEntries.Length);

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
            var config = GetConfig(entry.UnitType);
            if (config == null)
            {
                Debug.LogError($"[UnitSpawner] No configuration found for unit type: {entry.UnitType}");
                return null;
            }

            var unit = _container.InstantiatePrefabForComponent<Unit>(_baseUnitPrefab, Vector3.zero,
                Quaternion.identity, transform
            );

            ConfigureUnit(unit, config);
            ApplyScale(unit, scaleMultiplier);

            return unit;
        }

        private UnitConfigData GetConfig(string unitType)
        {
            return _unitConfigurationSo.Units.FirstOrDefault(u => u.UnitType == unitType);
        }

        private void ConfigureUnit(Unit unit, UnitConfigData config)
        {
            unit.Initialization(config.UnitAnimationController, config.UnitType);
        }

        private void ApplyScale(Unit unit, float scaleMultiplier)
        {
            var scale = BaseUnitScale * scaleMultiplier;
            unit.transform.localScale = new Vector3(scale, scale, 1f);
        }

        private void PlaceUnitsOnGrid(List<Unit> units, UnitData[] unitEntries)
        {
            for (var i = 0; i < units.Count; i++)
            {
                var x = (int)unitEntries[i].CellX;
                var y = (int)unitEntries[i].CellY;

                _gridController.PlaceUnitAtCell(units[i], x, y);
            }
        }
    }
}