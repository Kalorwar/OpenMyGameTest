using System;
using DG.Tweening;
using Project.Scripts.ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Scripts.UI
{
    public class Balloon : MonoBehaviour
    {
        private const float OutScreenOffset = 1;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        private Sequence _animationSequence;
        private Camera _camera;
        public event Action OnDestroyed;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void OnDestroy()
        {
            OnDestroyed?.Invoke();
            _animationSequence.Kill();
        }

        public void Initialize(BalloonConfigSo configSo)
        {
            if (configSo.Sprites.Length > 0)
            {
                _spriteRenderer.sprite = configSo.Sprites[Random.Range(0, configSo.Sprites.Length)];
            }

            var scale = Random.Range(configSo.MinScale, configSo.MaxScale);
            transform.localScale = new Vector3(scale, scale, scale);
        }

        public void Launch(float directionX, float speed, float swayAmplitude, float swayFrequency)
        {
            var screenHalfWidth = _camera.orthographicSize * _camera.aspect;

            var startX = transform.position.x;
            var targetX = directionX > 0 ? screenHalfWidth + OutScreenOffset : -screenHalfWidth - OutScreenOffset;

            var distance = Mathf.Abs(targetX - startX);
            var duration = distance / speed;
            _animationSequence = DOTween.Sequence();
            _animationSequence.Join(transform.DOMoveX(targetX, duration).SetEase(Ease.Linear));
            var startY = transform.position.y;
            _animationSequence.Join(
                DOVirtual.Float(0, Mathf.PI * 2 * duration * swayFrequency, duration,
                    t =>
                    {
                        transform.position =
                            new Vector3(transform.position.x, startY + Mathf.Sin(t) * swayAmplitude, 0);
                    }).SetEase(Ease.Linear)
            );

            _animationSequence.OnComplete(() => Destroy(gameObject));
        }
    }
}