using Project.Scripts.Grid;
using Project.Scripts.Level;
using Project.Scripts.Other;
using UnityEngine;
using Zenject;

namespace Project.Scripts.ZenjectInstallers
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private LevelGridController _levelGridController;

        public override void InstallBindings()
        {
            Container.Bind<IPlayerInputState>().To<PlayerInputState>().AsSingle().NonLazy();
            Container.Bind<LevelGridController>().FromComponentInNewPrefab(_levelGridController).AsSingle().NonLazy();
            Container.Bind<ILevelDataProvider>().To<LevelDataProvider>().AsSingle().NonLazy();
        }
    }
}