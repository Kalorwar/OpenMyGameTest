using UnityEngine;

namespace Project.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "BalloonConfig", menuName = "Scriptable Objects/Balloon Config")]
    public class BalloonConfigSo : ScriptableObject
    {
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private float _minScale;
        [SerializeField] private float _maxScale;

        [SerializeField] private float _minSpeed;
        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _minSwayAmplitude;
        [SerializeField] private float _maxSwayAmplitude;
        [SerializeField] private float _minSwayFrequency;
        [SerializeField] private float _maxSwayFrequency;

        [SerializeField] private float _minHeight;
        [SerializeField] private float _maxHeight;
        [SerializeField] private float _spawnOffsetX;

        public Sprite[] Sprites => _sprites;
        public float MinScale => _minScale;
        public float MaxScale => _maxScale;
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