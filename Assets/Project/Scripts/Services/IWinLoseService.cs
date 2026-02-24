using System;
using System.Collections.Generic;
using Project.Scripts.Units;

namespace Project.Scripts.Services
{
    public interface IWinLoseService
    {
        public bool IsGameEnded { get; }
        public event Action OnWin;
        public void Initialize(List<Unit> units);
        public void ForceTriggerWin();
    }
}