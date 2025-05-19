using Project.Content.ObjectPool;
using Zenject;

namespace Project.Architecture
{
    public class ObjectPoolerInstaller : Installer<ObjectPoolerInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<PoolsParentContainer>().AsSingle().NonLazy();
            Container.Bind<FiltrablePoolsHandler>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<SimleProjectilePoolFactory>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<DestroyersPoolFactory>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<MainTargetAttackersPoolFactory>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<InfantrymenPoolFactory>().AsSingle().NonLazy();

            Container.Bind<FloatingTextPoolFactory>().AsSingle().NonLazy();
        }
    }
}