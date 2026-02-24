using Project.Scripts.GlobalContext;
using Project.Scripts.Services;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Scripts.UI
{
    [RequireComponent(typeof(Button))]
    public class RestartButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        private int _currentLevel;
        private ISaveLoadService _saveLoadService;
        private ISceneController _sceneController;

        [Inject]
        public void Construct(ISceneController sceneController, ISaveLoadService saveLoadService)
        {
            _sceneController = sceneController;
            _saveLoadService = saveLoadService;
        }

        private void Start()
        {
            _currentLevel = _saveLoadService.CurrentLevelId;
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(RestartLevel);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(RestartLevel);
        }

        private void RestartLevel()
        {
            if (_currentLevel < _saveLoadService.CurrentLevelId)
            {
                _saveLoadService.SetCurrentLevel(_currentLevel);
            }

            _saveLoadService.ClearSavedLevel();
            _sceneController.ResetLevel();
        }
    }
}