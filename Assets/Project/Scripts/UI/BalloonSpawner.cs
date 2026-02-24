using System.Collections;
using Project.Scripts.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace Project.Scripts.UI
{
    public class BalloonSpawner : MonoBehaviour
    {
        private const float MinSpawnDelay = 2f;
        private const float MaxSpawnDelay = 5f;
        [SerializeField] private Balloon _balloonPrefab;
        [SerializeField] private int _maxBalloons = 3;
        private Camera _camera;
        private BalloonConfigSo _configSo;

        private int _currentBalloonCount;

        [Inject]
        private void Construct(BalloonConfigSo configSo)
        {
            _configSo = configSo;
        }

        private void Awake()
        {
            _camera = Camera.main;
        }

        public void StartSpawning()
        {
            StartCoroutine(SpawnLoop());
        }

        private IEnumerator SpawnLoop()
        {
            while (true)
            {
                if (_currentBalloonCount < _maxBalloons)
                {
                    SpawnBalloon();
                }

                var delay = Random.Range(MinSpawnDelay, MaxSpawnDelay);
                yield return new WaitForSeconds(delay);
            }
        }

        private void SpawnBalloon()
        {
            var direction = Random.value > 0.5f ? 1f : -1f;

            var height = Random.Range(_configSo.MinHeight, _configSo.MaxHeight);

            var screenWidth = _camera.orthographicSize * _camera.aspect;
            var startX = direction > 0 ? -screenWidth - _configSo.SpawnOffsetX : screenWidth + _configSo.SpawnOffsetX;

            var spawnPos = new Vector3(startX, height, 0);

            var balloon = Instantiate(_balloonPrefab, spawnPos, Quaternion.identity, transform);
            _currentBalloonCount++;

            balloon.GetComponent<Balloon>().OnDestroyed += () => _currentBalloonCount -= 1;
            balloon.Initialize(_configSo);

            var speed = Random.Range(_configSo.MinSpeed, _configSo.MaxSpeed);
            var swayAmplitude = Random.Range(_configSo.MinSwayAmplitude, _configSo.MaxSwayAmplitude);
            var swayFrequency = Random.Range(_configSo.MinSwayFrequency, _configSo.MaxSwayFrequency);

            balloon.Launch(direction, speed, swayAmplitude, swayFrequency);
        }
    }
}