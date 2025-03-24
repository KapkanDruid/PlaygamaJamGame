using Project.Content.CharacterAI;
using UnityEngine;
using Zenject;


namespace Project.Content.BuildSystem
{
    public class BarracksInstaller : MonoInstaller
    {
        [SerializeField] private BarracksEntity _entity;
        [SerializeField] private HealthBarView _healthView;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GridPlaceComponent>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<BarracksSpawnLogic>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<BarracksEntity>().FromComponentOnRoot().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<BarracksData>().FromInstance(_entity.Data).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<BuildingHealthComponent>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<HealthBarView>().FromInstance(_healthView).AsSingle().NonLazy();
        }
    }
}