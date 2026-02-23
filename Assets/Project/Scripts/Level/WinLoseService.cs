using System;
using System.Collections.Generic;
using Project.Scripts.Services;
using Project.Scripts.Units;
using UnityEngine;

namespace Project.Scripts.Level
{
    public class WinLoseService : IWinLoseService, IDisposable
    {
        private List<Unit> _trackedUnits = new();

        public void Dispose()
        {
            UnsubscribeFromUnits();
        }

        public bool IsGameEnded { get; private set; }

        public event Action OnWin;
        public event Action OnLose;

        public void Initialize(List<Unit> units)
        {
            if (units == null || units.Count == 0)
            {
                Debug.LogWarning("[WinLoseController] No units to track");
                TriggerWin();
                return;
            }

            _trackedUnits = new List<Unit>(units);
            IsGameEnded = false;

            SubscribeToUnits();
        }

        public void ForceTriggerWin()
        {
            TriggerWin();
        }

        private void SubscribeToUnits()
        {
            foreach (var unit in _trackedUnits)
            {
                if (unit != null)
                {
                    unit.OnUnitDestroyed += HandleUnitDestroyed;
                }
            }
        }

        private void UnsubscribeFromUnits()
        {
            foreach (var unit in _trackedUnits)
            {
                if (unit != null)
                {
                    unit.OnUnitDestroyed -= HandleUnitDestroyed;
                }
            }
        }

        private void HandleUnitDestroyed(Unit destroyedUnit)
        {
            if (IsGameEnded)
            {
                return;
            }

            destroyedUnit.OnUnitDestroyed -= HandleUnitDestroyed;
            _trackedUnits.Remove(destroyedUnit);
            CheckWinCondition();
        }

        private void CheckWinCondition()
        {
            if (_trackedUnits.Count == 0)
            {
                TriggerWin();
            }
        }

        private void TriggerWin()
        {
            if (IsGameEnded)
            {
                return;
            }

            IsGameEnded = true;
            UnsubscribeFromUnits();
            OnWin?.Invoke();
        }
    }
}