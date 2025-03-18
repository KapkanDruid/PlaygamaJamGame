using NavMeshPlus.Components;
using Project.Content;
using Project.Content.BuildSystem;
using UnityEngine;
using Zenject;

namespace Project.Architecture
{
    public class MainServicesSceneInstaller : MonoInstaller
    {
        [SerializeField] private MainSceneBootstrap _sceneBootstrap;
        [SerializeField] private GridPlaceSystem _gridPlaceSystem;
        [SerializeField] private GizmosDrawer _gizmosDrawer;
        [SerializeField] private Grid _grid;
        [SerializeField] private NavMeshSurface _navMeshSurface;
        [SerializeField] private SceneRecourses _recourses;

        public override void InstallBindings()
        {
            Container.Bind<MainSceneBootstrap>().FromInstance(_sceneBootstrap).AsSingle().NonLazy();
            Container.Bind<GridPlaceSystem>().FromInstance(_gridPlaceSystem).AsSingle().NonLazy();
            Container.Bind<Grid>().FromInstance(_grid).AsSingle().NonLazy();
            Container.Bind<InputSystemActions>().AsSingle().NonLazy();
            Container.Bind<Camera>().FromInstance(Camera.main).AsSingle().NonLazy();
            Container.Bind<GizmosDrawer>().FromInstance(_gizmosDrawer).AsSingle().NonLazy();
            Container.Bind<NavMeshSurface>().FromInstance(_navMeshSurface).AsSingle().NonLazy();
            Container.Bind<SceneRecourses>().FromInstance(_recourses).AsSingle().NonLazy();

            FactoriesInstaller.Install(Container);
        }
    }
}