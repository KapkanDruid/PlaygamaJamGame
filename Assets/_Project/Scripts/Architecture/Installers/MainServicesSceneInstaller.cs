using Project.Content.BuildSystem;
using UnityEngine;
using Zenject;

namespace Project.Architecture
{
    public class MainServicesSceneInstaller : MonoInstaller
    {
        [SerializeField] private MainSceneBootstrap _sceneBootstrap;

        public override void InstallBindings()
        {
            Container.Bind<MainSceneBootstrap>().FromInstance(_sceneBootstrap).AsSingle();
            Container.Bind<InputSystemActions>().AsSingle().NonLazy();
        }
    }
}