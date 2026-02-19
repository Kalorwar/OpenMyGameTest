using Project.Scripts.Level;
using Zenject;

namespace Project.Scripts.ZenjectInstallers
{
    public class LevelInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ILevelDataProvider>().To<LevelDataProvider>().AsSingle().NonLazy();
        }
    }
}