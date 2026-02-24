using Project.Scripts.Grid;
using Project.Scripts.Level;
using Project.Scripts.Other;
using Project.Scripts.ScriptableObjects;
using Project.Scripts.Services;
using Project.Scripts.UI;
using UnityEngine;
using Zenject;

namespace Project.Scripts.ZenjectInstallers
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private LevelGridController _levelGridController;
        [SerializeField] private UnitConfigurationSo _unitConfigurationSo;
        [SerializeField] private UnitSpawner _unitSpawner;
        [SerializeField] private GameSessionController _gameSessionController;
        [SerializeField] private BalloonConfigSo _balloonConfigSo;
        [SerializeField] private BalloonSpawner _balloonSpawner;

        public override void InstallBindings()
        {
            Container.Bind<UnitConfigurationSo>().FromInstance(_unitConfigurationSo).AsSingle().NonLazy();
            Container.Bind<BalloonConfigSo>().FromInstance(_balloonConfigSo).AsSingle().NonLazy();
            Container.Bind<IPlayerInputState>().To<PlayerInputState>().AsSingle().NonLazy();
            Container.Bind<ISaveLoadService>().To<SaveLoadService>().AsSingle().NonLazy();
            Container.Bind<ILevelDataProvider>().To<LevelDataProvider>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<WinLoseService>().AsSingle().NonLazy();
            Container.Bind<LevelGridController>().FromComponentInNewPrefab(_levelGridController).AsSingle().NonLazy();
            Container.Bind<UnitSpawner>().FromComponentInNewPrefab(_unitSpawner).AsSingle().NonLazy();
            Container.Bind<GameSessionController>().FromComponentInNewPrefab(_gameSessionController).AsSingle()
                .NonLazy();
            Container.Bind<BalloonSpawner>().FromComponentInNewPrefab(_balloonSpawner).AsSingle().NonLazy();
        }
    }
}