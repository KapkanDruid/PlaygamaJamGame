using Project.Content;
using UnityEngine;
using Zenject;

namespace Project.Architecture
{
    public class MainMenuInstaller : MonoInstaller
    {
        [SerializeField] private SceneRecourses _sceneRecourses;

        public override void InstallBindings()
        {
            Container.Bind<SceneRecourses>().FromInstance(_sceneRecourses).AsSingle().NonLazy();
            Container.Bind<AudioController>().FromComponentInHierarchy().AsSingle().NonLazy();
        }

    }
}
