using Project.Content;
using Project.Content.BuildSystem;
using Zenject;

namespace Project.Architecture
{
    public class FactoriesInstaller : Installer<FactoriesInstaller>
    {
        [Inject] private SceneRecourses _recourses;

        public override void InstallBindings()
        {
            Container.BindFactory<MainBuildingEntity, MainBuildingEntity.Factory>()
                .FromSubContainerResolve()
                .ByNewContextPrefab(_recourses.Prefabs.MainBuildingFirstLevel);
        }
    }
}