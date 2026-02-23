using UnityEngine;

namespace Project.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Scriptable Objects/Game Settings")]
    public class GameSettingsSo : ScriptableObject
    {
        [SerializeField] private int _targetFPS = 60;

        public int TargetFPS => _targetFPS;
    }
}