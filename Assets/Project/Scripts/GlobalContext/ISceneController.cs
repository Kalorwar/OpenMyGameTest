using System;

namespace Project.Scripts.GlobalContext
{
    public interface ISceneController
    {
        public event Action OnLevelRestart;
        public void LoadScene(string levelName);
        public void ResetLevel();
    }
}