using UnityEngine;

namespace Project.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "BalloonConfig", menuName = "Scriptable Objects/Balloon Config")]
    public class BalloonConfigSo : ScriptableObject
    {
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private float _minScale = 0.8f;
        [SerializeField] private float _maxScale = 1.1f;
        [SerializeField] private float _spawnDuration = 0.5f;

        [SerializeField] private float _minSpeed = 0.5f;
        [SerializeField] private float _maxSpeed = 1.5f;
        [SerializeField] private float _minSwayAmplitude = 0.3f;
        [SerializeField] private float _maxSwayAmplitude = 0.8f;
        [SerializeField] private float _minSwayFrequency = 0.2f;
        [SerializeField] private float _maxSwayFrequency = 0.5f;

        [SerializeField] private float _minHeight = -2f;
        [SerializeField] private float _maxHeight = 3f;
        [SerializeField] private float _spawnOffsetX = 3f;

        public Sprite[] Sprites => _sprites;
        public float MinScale => _minScale;
        public float MaxScale => _maxScale;
        public float SpawnDuration => _spawnDuration;
        public float MinSpeed => _minSpeed;
        public float MaxSpeed => _maxSpeed;
        public float MinSwayAmplitude => _minSwayAmplitude;
        public float MaxSwayAmplitude => _maxSwayAmplitude;
        public float MinSwayFrequency => _minSwayFrequency;
        public float MaxSwayFrequency => _maxSwayFrequency;
        public float MinHeight => _minHeight;
        public float MaxHeight => _maxHeight;
        public float SpawnOffsetX => _spawnOffsetX;
    }
}