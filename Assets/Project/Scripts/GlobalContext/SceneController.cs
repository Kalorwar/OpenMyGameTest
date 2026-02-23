using System;
using UnityEngine.SceneManagement;

namespace Project.Scripts.GlobalContext
{
    public class SceneController : ISceneController
    {
        public event Action OnLevelRestart;

        public void LoadScene(string levelName)
        {
            SceneManager.LoadScene(levelName);
        }

        public void ResetLevel()
        {
            OnLevelRestart?.Invoke();
            LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}