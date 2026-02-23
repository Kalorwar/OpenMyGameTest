using Project.Scripts.Grid;
using Project.Scripts.Level;
using Project.Scripts.Other;
using Project.Scripts.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace Project.Scripts.ZenjectInstallers
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private LevelGridController _levelGridController;
        [SerializeField] private UnitConfiguration _unitConfiguration;

        public override void InstallBindings()
        {
            Container.Bind<IPlayerInputState>().To<PlayerInputState>().AsSingle().NonLazy();
            Container.Bind<LevelGridController>().FromComponentInNewPrefab(_levelGridController).AsSingle().NonLazy();
            Container.Bind<ILevelDataProvider>().To<LevelDataProvider>().AsSingle().NonLazy();
            Container.Bind<UnitConfiguration>().FromInstance(_unitConfiguration).AsSingle().NonLazy();
        }
    }
}