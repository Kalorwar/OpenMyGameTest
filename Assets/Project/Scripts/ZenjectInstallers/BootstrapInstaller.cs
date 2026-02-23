using Project.Scripts.GlobalContext;
using Project.Scripts.Other;
using Project.Scripts.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace Project.Scripts.ZenjectInstallers
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private GameSettingsSo _gameSettingsSo;

        public override void InstallBindings()
        {
            Container.Bind<GameSettingsSo>().FromInstance(_gameSettingsSo).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<SceneController>().AsSingle().NonLazy();
            Container.Bind<TargetFPSLocker>().AsSingle().NonLazy();
        }
    }
}