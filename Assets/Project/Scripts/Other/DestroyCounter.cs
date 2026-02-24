using System;
using System.Threading;

namespace Project.Scripts.Other
{
    public class DestroyCounter
    {
        private readonly Action _onComplete;
        private int _remaining;

        public DestroyCounter(int total, Action onComplete)
        {
            _remaining = total;
            _onComplete = onComplete;
        }

        public void Decrement()
        {
            if (Interlocked.Decrement(ref _remaining) == 0)
            {
                _onComplete?.Invoke();
            }
        }
    }
}