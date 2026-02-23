using System;
using DG.Tweening;
using Project.Scripts.ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Scripts.UI
{
    public class Balloon : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        private Tween _moveTween;
        private Tween _scaleTween;
        private Tween _swayTween;
        public event Action OnDestroyed;

        private void OnDestroy()
        {
            OnDestroyed?.Invoke();
            _moveTween?.Kill();
            _swayTween?.Kill();
            _scaleTween?.Kill();
        }

        public void Initialize(BalloonConfigSo configSo)
        {
            if (configSo.Sprites.Length > 0)
            {
                spriteRenderer.sprite = configSo.Sprites[Random.Range(0, configSo.Sprites.Length)];
            }

            var scale = Random.Range(configSo.MinScale, configSo.MaxScale);
            transform.localScale = Vector3.zero;
            transform.DOScale(scale, configSo.SpawnDuration).SetEase(Ease.OutBack);
            spriteRenderer.color = new Color(1, 1, 1, Random.Range(0.7f, 1f));
        }

        public void Launch(float directionX, float speed, float swayAmplitude, float swayFrequency)
        {
            var screenWidth = Camera.main.orthographicSize * Camera.main.aspect * 2;
            var targetX = directionX > 0 ? screenWidth + 2 : -screenWidth - 2;
            var distance = Mathf.Abs(targetX - transform.position.x);
            var duration = distance / speed;

            _moveTween = transform.DOMoveX(targetX, duration).SetEase(Ease.Linear)
                .OnComplete(() => Destroy(gameObject));
            var startY = transform.position.y;

            _swayTween = DOVirtual.Float(0, Mathf.PI * 2 * duration * swayFrequency, duration, value =>
            {
                var offsetY = Mathf.Sin(value) * swayAmplitude;
                transform.position = new Vector3(transform.position.x, startY + offsetY, transform.position.z);
            }).SetEase(Ease.Linear);

            _scaleTween = transform.DOScale(transform.localScale * 1.05f, 1f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }
    }
}