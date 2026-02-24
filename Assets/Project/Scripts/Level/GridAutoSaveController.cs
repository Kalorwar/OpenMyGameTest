using System;
using Project.Scripts.Datas;
using Project.Scripts.Grid;
using Project.Scripts.Services;

namespace Project.Scripts.Level
{
    public class GridAutoSaveController
    {
        private readonly Func<bool> _canSavePredicate;
        private readonly ILevelDataProvider _levelDataProvider;
        private readonly LevelGridController _levelGridController;
        private readonly float _saveDelay;
        private readonly ISaveLoadService _saveLoadService;

        private bool _savePending;
        private float _saveTimer;

        public GridAutoSaveController(float saveDelay, ILevelDataProvider levelDataProvider,
            LevelGridController levelGridController, ISaveLoadService saveLoadService, Func<bool> canSavePredicate)
        {
            _saveDelay = saveDelay;
            _levelDataProvider = levelDataProvider;
            _levelGridController = levelGridController;
            _saveLoadService = saveLoadService;
            _canSavePredicate = canSavePredicate;

            _levelGridController.OnGridChanged += RequestSave;
        }

        public void Dispose()
        {
            _levelGridController.OnGridChanged -= RequestSave;
        }

        public void Update(float deltaTime)
        {
            if (!_savePending)
            {
                return;
            }

            _saveTimer -= deltaTime;
            if (_saveTimer <= 0)
            {
                _savePending = false;
                TrySave();
            }
        }

        private void RequestSave()
        {
            if (!_canSavePredicate())
            {
                return;
            }

            _savePending = true;
            _saveTimer = _saveDelay;
        }

        private void TrySave()
        {
            if (!_canSavePredicate())
            {
                return;
            }

            SaveGrid();
        }

        private void SaveGrid()
        {
            var savedLevelData = new LevelData
            {
                Width = _levelDataProvider.CurrentLevelData.Width,
                Height = _levelDataProvider.CurrentLevelData.Height,
                Units = _levelGridController.GetCurrentUnitDatas()
            };
            _saveLoadService.Save(savedLevelData);
        }
    }
}