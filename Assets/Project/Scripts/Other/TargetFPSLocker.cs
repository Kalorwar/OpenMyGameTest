using Project.Scripts.ScriptableObjects;
using UnityEngine;

namespace Project.Scripts.Other
{
    public class TargetFPSLocker
    {
        public TargetFPSLocker(GameSettingsSo gameSettings)
        {
            Application.targetFrameRate = gameSettings.TargetFPS;
        }
    }
}