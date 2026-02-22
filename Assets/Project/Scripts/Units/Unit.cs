using System;
using System.Collections;
using DG.Tweening;
using Project.Scripts.Grid;
using Project.Scripts.Other;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Project.Scripts.Units
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Unit : MonoBehaviour, IPointerDownHandler, IDragHandler
    {
        private const float MoveAnimationDuration = 0.3f;
        private const float DestroyScaleDuration = 0.3f;

        [SerializeField] private ElementType _elementType;
        [SerializeField] private Animator _animator;
        private Coroutine _destroyAnimationCoroutine;

        private SpriteRenderer _spriteRenderer;
        private SwipeHandler _swipeHandler;
        private UnitAnimationController _unitAnimationController;

        [Inject]
        public void Construct(LevelGridController grid, IPlayerInputState playerInputState)
        {
            _swipeHandler = new SwipeHandler(this, grid, playerInputState);
        }

        public ElementType ElementType => _elementType;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _unitAnimationController = new UnitAnimationController(_animator);
        }

        private void OnDestroy()
        {
            StopStartedCoroutines();
            transform.DOKill();
        }

        public void OnDrag(PointerEventData eventData)
        {
            _swipeHandler?.OnDrag(eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _swipeHandler?.OnPointerDown(eventData);
        }

        public void ChangeSortOrder(int sortOrder)
        {
            _spriteRenderer.sortingOrder = sortOrder;
        }

        public void AnimateMoveTo(Vector3 worldPosition, int sortOrder, Action callback = null)
        {
            ChangeSortOrder(sortOrder);
            transform.DOMove(worldPosition, MoveAnimationDuration)
                .OnComplete(() => callback?.Invoke());
        }

        public void Destroy(Action callback = null)
        {
            if (_destroyAnimationCoroutine != null)
            {
                return;
            }

            _unitAnimationController.AnimateDestroy();
            _destroyAnimationCoroutine = StartCoroutine(DestroyAnimationSequence(callback));
        }

        private IEnumerator DestroyAnimationSequence(Action callback)
        {
            yield return new WaitForSeconds(_unitAnimationController.DestroyAnimationDuration);

            yield return transform.DOScale(Vector3.zero, DestroyScaleDuration)
                .WaitForCompletion();

            Destroy(gameObject);
            callback?.Invoke();
            _destroyAnimationCoroutine = null;
        }

        private void StopStartedCoroutines()
        {
            if (_destroyAnimationCoroutine != null)
            {
                StopCoroutine(_destroyAnimationCoroutine);
                _destroyAnimationCoroutine = null;
            }
        }
    }
}