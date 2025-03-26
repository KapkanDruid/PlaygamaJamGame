using UnityEngine;
using Zenject;

namespace Project.Content.BuildSystem
{
    public class WallInstaller : MonoInstaller
    {
        [SerializeField] private WallEntity _entity;
        [SerializeField] private HealthBarView _healthView;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GridPlaceComponent>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<WallEntity>().FromComponentOnRoot().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<WallData>().FromInstance(_entity.Data).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<BuildingHealthComponent>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<HealthBarView>().FromInstance(_healthView).AsSingle().NonLazy();
        }
    }
}
