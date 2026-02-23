using Project.Scripts.Datas;
using Project.Scripts.GlobalContext;
using Project.Scripts.Grid;
using Project.Scripts.Services;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Level
{
    public class GameSessionController : MonoBehaviour
    {
        private bool _isRestarting;
        private ILevelDataProvider _levelDataProvider;
        private LevelGridController _levelGridController;
        private ISaveLoadService _saveLoadService;
        private ISceneController _sceneController;
        private UnitSpawner _unitSpawner;
        private IWinLoseService _winLoseService;

        [Inject]
        private void Construct(UnitSpawner unitSpawner, LevelGridController levelGridController,
            ISaveLoadService saveLoadService, ILevelDataProvider levelDataProvider, IWinLoseService winLoseService,
            ISceneController sceneController)
        {
            _unitSpawner = unitSpawner;
            _levelGridController = levelGridController;
            _saveLoadService = saveLoadService;
            _levelDataProvider = levelDataProvider;
            _winLoseService = winLoseService;
            _sceneController = sceneController;
        }

        private void Awake()
        {
            _levelGridController.InitializeGrid();
        }

        private void Start()
        {
            _unitSpawner.SpawnUnits();
            _winLoseService.Initialize(_unitSpawner.SpawnedUnits);
            _levelGridController.TryNormalize();
        }

        private void OnEnable()
        {
            _winLoseService.OnWin += OnLevelWin;
            _sceneController.OnLevelRestart += PrepareForRestart;
        }

        private void OnDisable()
        {
            _winLoseService.OnWin -= OnLevelWin;
            _sceneController.OnLevelRestart -= PrepareForRestart;
        }

        private void OnApplicationQuit()
        {
            if (!_isRestarting && !_winLoseService.IsGameEnded)
            {
                SaveGrid();
            }
        }

        private void PrepareForRestart()
        {
            _isRestarting = true;
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

        private void OnLevelWin()
        {
            _saveLoadService.ClearSavedLevel();
            _saveLoadService.IncreaseCurrentLevel();
        }
    }
}