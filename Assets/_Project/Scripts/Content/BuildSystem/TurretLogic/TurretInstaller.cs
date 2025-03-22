using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using Zenject;

namespace Project.Content.BuildSystem
{
    public class TurretInstaller : MonoInstaller
    {
        [Inject] private TurretDynamicData _dynamicData;
        [SerializeField] private TurretEntity _entity;
        [SerializeField] private HealthBarView _healthView;

        public override void InstallBindings()
        {
            Container.Bind<TurretDynamicData>().FromInstance(_dynamicData).AsSingle();

            Container.BindInterfacesAndSelfTo<GridPlaceComponent>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<BuildingHealthComponent>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<HealthBarView>().FromInstance(_healthView).AsSingle().NonLazy();
            Container.Bind<GizmosDrawer>().FromNewComponentOnRoot().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<TurretAttackComponent>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<TurretData>().FromInstance(_entity.Data).AsSingle().NonLazy();
            Container.Bind<CancellationToken>().FromInstance(_entity.GetCancellationTokenOnDestroy()).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<TurretEntity>().FromComponentOnRoot().AsSingle().NonLazy();
        }
    }
}
