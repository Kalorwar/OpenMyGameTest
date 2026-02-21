using DG.Tweening;
using Project.Scripts.Other;
using UnityEngine;

namespace Project.Scripts.Units
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Unit : MonoBehaviour
    {
        private const float MoveAnimationDuration = 0.3f;
        [SerializeField] private ElementType _elementType;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void ChangeSortOrder(int sortOrder)
        {
            _spriteRenderer.sortingOrder = sortOrder;
        }

        public void AnimateMoveTo(Vector3 worldPosition, int sortOrder)
        {
            ChangeSortOrder(sortOrder);
            transform.DOMove(worldPosition, MoveAnimationDuration);
        }
    }
}