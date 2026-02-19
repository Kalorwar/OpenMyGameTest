using Project.Scripts.Other;
using UnityEngine;

namespace Project.Scripts.Units
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Unit : MonoBehaviour
    {
        [SerializeField] private ElementType _elementType;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Initialize(int sortOrder)
        {
            _spriteRenderer.sortingOrder = sortOrder;
        }
    }
}