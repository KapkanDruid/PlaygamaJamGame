using NavMeshPlus.Components;
using Project.Content;
using Project.Content.BuildSystem;
using Project.Content.CharacterAI;
using Project.Content.CoreGameLoopLogic;
using Project.Content.UI;
using UnityEngine;
using Zenject;

namespace Project.Architecture
{
    public class MainServicesSceneInstaller : MonoInstaller
    {
        [SerializeField] private MainSceneBootstrap _sceneBootstrap;
        [SerializeField] private GizmosDrawer _gizmosDrawer;
        [SerializeField] private Grid _grid;
        [SerializeField] private NavMeshSurface _navMeshSurface;
        [SerializeField] private SceneRecourses _recourses;
        [SerializeField] private SceneData _sceneData;
        [SerializeField] private CardsPopupView _cardsPopupView;
        [SerializeField] private LevelExperienceView _experienceView;
        [SerializeField] private DefensiveFlag _defensiveFlag;
        [SerializeField] private AudioController _audioController;
        [SerializeField] private AlertController _alertController;

        public override void InstallBindings()
        {
            Container.Bind<MainSceneBootstrap>().FromInstance(_sceneBootstrap).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GridPlaceSystem>().AsSingle().NonLazy();
            Container.Bind<Grid>().FromInstance(_grid).AsSingle().NonLazy();
            Container.Bind<InputSystemActions>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PauseHandler>().AsSingle().NonLazy();
            Container.Bind<Camera>().FromInstance(Camera.main).AsSingle().NonLazy();
            Container.Bind<GizmosDrawer>().FromInstance(_gizmosDrawer).AsSingle().NonLazy();
            Container.Bind<NavMeshSurface>().FromInstance(_navMeshSurface).AsSingle().NonLazy();
            Container.Bind<SceneRecourses>().FromInstance(_recourses).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<SceneData>().FromInstance(_sceneData).AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<EntityCommander>().AsSingle().NonLazy();
            Container.Bind<BuildingSpawner>().AsSingle().NonLazy();

            Container.Bind<CardsPopupView>().FromInstance(_cardsPopupView).AsSingle().NonLazy();
            Container.Bind<CardsPopupPresenter>().FromComponentOn(_cardsPopupView.gameObject).AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<LevelExperienceController>().AsSingle().NonLazy();
            Container.Bind<LevelExperienceView>().FromInstance(_experienceView).AsSingle().NonLazy();
            Container.Bind<DefensiveFlag>().FromInstance(_defensiveFlag).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<WinLoseHandler>().AsSingle().NonLazy();

            Container.Bind<FloatingTextHandler>().AsSingle().NonLazy();

            Container.Bind<UpgradeEffectController>().AsSingle().NonLazy();

            Container.Bind<AudioController>().FromInstance(_audioController).AsSingle().NonLazy();
            Container.Bind<AlertController>().FromInstance(_alertController).AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<TimedClipPlayer>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<SkipHandler>().AsTransient().NonLazy();

            FactoriesInstaller.Install(Container);
        }
    }
}