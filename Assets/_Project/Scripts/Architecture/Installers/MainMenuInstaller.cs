using Project.Content;
using Project.Content.CoreGameLoopLogic;
using UnityEngine;
using Zenject;

namespace Project.Architecture
{
    public class MainMenuInstaller : MonoInstaller
    {
        [SerializeField] private SceneRecourses _sceneRecourses;
        [SerializeField] private MainMenuData _menuData;

        public override void InstallBindings()
        {
            Container.Bind<SceneRecourses>().FromInstance(_sceneRecourses).AsSingle().NonLazy();
            Container.Bind<AudioController>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<MainMenuData>().FromInstance(_menuData).AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<SkipHandler>().AsTransient().NonLazy();
            Container.Bind<InputSystemActions>().AsSingle().NonLazy();
        }
    }
}
