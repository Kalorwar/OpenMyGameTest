using System.Collections.Generic;
using Project.Scripts.Datas;
using UnityEngine;

namespace Project.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "UnitConfiguration", menuName = "Scriptable Objects/Unit Configuration")]
    public class UnitConfigurationSo : ScriptableObject
    {
        [SerializeField] private List<UnitConfigData> _units;

        public List<UnitConfigData> Units => _units;
    }
}