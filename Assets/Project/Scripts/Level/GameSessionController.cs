using Project.Scripts.GlobalContext;
using Project.Scripts.Grid;
using Project.Scripts.Services;
using Project.Scripts.UI;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Level
{
    public class GameSessionController : MonoBehaviour
    {
        private const float SaveDelay = 0.3f;

        private GridAutoSaveController _autoSaveController;
        private BalloonSpawner _balloonSpawner;
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
            ISceneController sceneController, BalloonSpawner balloonSpawner)
        {
            _unitSpawner = unitSpawner;
            _levelGridController = levelGridController;
            _saveLoadService = saveLoadService;
            _levelDataProvider = levelDataProvider;
            _winLoseService = winLoseService;
            _sceneController = sceneController;
            _balloonSpawner = balloonSpawner;
        }

        private void Awake()
        {
            _levelGridController.InitializeGrid();
            _autoSaveController = new GridAutoSaveController(SaveDelay, _levelDataProvider, _levelGridController,
                _saveLoadService, CanSavePredicate);
        }

        private void Start()
        {
            _unitSpawner.SpawnUnits();
            _balloonSpawner.StartSpawning();
            _winLoseService.Initialize(_unitSpawner.SpawnedUnits);
            _levelGridController.TryNormalize();
        }

        private void Update()
        {
            _autoSaveController?.Update(Time.deltaTime);
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

        private void OnDestroy()
        {
            _autoSaveController?.Dispose();
        }

        private void PrepareForRestart()
        {
            _isRestarting = true;
        }

        private void OnLevelWin()
        {
            _saveLoadService.ClearSavedLevel();
            _saveLoadService.IncreaseCurrentLevel();
        }

        private bool CanSavePredicate()
        {
            return !_isRestarting && !_winLoseService.IsGameEnded;
        }
    }
}