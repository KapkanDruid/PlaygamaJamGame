using Project.Content;
using Project.Content.BuildSystem;
using Project.Content.CharacterAI;
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

            Container.BindFactory<TurretEntity, TurretEntity.Factory>()
                .WithFactoryArguments(TurretType.VoiceOfTruth)
                .FromSubContainerResolve()
                .ByNewContextPrefab(_recourses.Prefabs.VoiceOfTruthTurret);

            Container.BindFactory<BarracksEntity, BarracksEntity.Factory>()
                .WithFactoryArguments(BarracksType.Infantryman)
                .FromSubContainerResolve()
                .ByNewContextPrefab(_recourses.Prefabs.InfantrymanBarracks);

            Container.BindFactory<DefensiveFlag, DefensiveFlag.Factory>()
                .FromSubContainerResolve()
                .ByNewContextPrefab(_recourses.Prefabs.Flag);

            Container.BindFactory<WallEntity, WallEntity.Factory>()
                .FromSubContainerResolve()
                .ByNewContextPrefab(_recourses.Prefabs.WallPrefab);
        }
    }
}