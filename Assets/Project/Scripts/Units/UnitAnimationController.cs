using UnityEngine;

namespace Project.Scripts.Units
{
    public class UnitAnimationController
    {
        private static readonly int DestroyAnimationName = Animator.StringToHash("Destroy");
        private readonly Animator _animator;

        public UnitAnimationController(Animator animator)
        {
            _animator = animator;
        }

        public float DestroyAnimationDuration => 0.8f;

        public void AnimateDestroy()
        {
            _animator.SetTrigger(DestroyAnimationName);
        }
    }
}