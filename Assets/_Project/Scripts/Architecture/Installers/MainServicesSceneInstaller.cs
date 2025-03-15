using UnityEngine;
using Zenject;

namespace Assets.Scripts.Architecture
{
    public class MainServicesSceneInstaller : MonoInstaller
    {
        [SerializeField] private MainSceneBootstrap _sceneBootstrap;
        [SerializeField] private Canvas _levelCanvas;

        public override void InstallBindings()
        {
            Container.Bind<MainSceneBootstrap>().FromInstance(_sceneBootstrap).AsSingle();

            Container.Bind<Canvas>().FromInstance(_levelCanvas).AsSingle().NonLazy();

        }
    }
}