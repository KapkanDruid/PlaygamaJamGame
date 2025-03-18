using UnityEngine;
using Zenject;

namespace Project.Content.BuildSystem
{
    public class MainBuildingInstaller : MonoInstaller
    {
        [SerializeField] private MainBuildingEntity _entity;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GridPlaceComponent>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<MainBuildingEntity>().FromComponentOnRoot().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<MainBuildingData>().FromInstance(_entity.Data).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<BuildingHealthComponent>().AsSingle().NonLazy();
        }
    }
}