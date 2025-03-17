using UnityEngine;
using Zenject;

namespace Project.Content.BuildSystem
{
    public class MainBuildingInstaller : MonoInstaller
    {
        [SerializeField] private MainBuildingEntity _entity;

        public override void InstallBindings()
        {
            Debug.Log("MainBuildingInstaller");
            Container.Bind<GridPlaceComponent>().AsSingle().NonLazy();
            Container.Bind<MainBuildingEntity>().FromComponentOnRoot().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<MainBuildingData>().FromInstance(_entity.Data).AsSingle().NonLazy();
        }
    }
}