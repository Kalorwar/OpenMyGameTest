using System;
using UnityEngine;

namespace Project.Scripts.Datas
{
    [Serializable]
    public class UnitConfigData
    {
        [SerializeField] private RuntimeAnimatorController _unitAnimationController;
        [SerializeField] private string _unitType;

        public RuntimeAnimatorController UnitAnimationController => _unitAnimationController;
        public string UnitType => _unitType;
    }
}