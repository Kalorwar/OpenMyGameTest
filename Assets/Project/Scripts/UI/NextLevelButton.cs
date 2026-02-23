using Project.Scripts.GlobalContext;
using Project.Scripts.Services;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Scripts.UI
{
    [RequireComponent(typeof(Button))]
    public class NextLevelButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        private ISceneController _sceneController;
        private IWinLoseService _winLoseService;

        [Inject]
        public void Construct(ISceneController sceneController, IWinLoseService winLoseService)
        {
            _sceneController = sceneController;
            _winLoseService = winLoseService;
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(StartNextLevel);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(StartNextLevel);
        }

        private void StartNextLevel()
        {
            _winLoseService.ForceTriggerWin();
            _sceneController.LoadScene(SceneLibrary.LevelName);
        }
    }
}