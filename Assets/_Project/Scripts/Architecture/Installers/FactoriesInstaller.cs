using Project.Content;
using Project.Content.BuildSystem;
using Project.Content.CharacterAI.Destroyer;
using Project.Content.CharacterAI.MainTargetAttacker;
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

            Container.BindFactory<DestroyerHandler, DestroyerHandler.Factory>()
                .FromSubContainerResolve()
                .ByNewContextPrefab(_recourses.Prefabs.Destroyer);

            Container.BindFactory<MainTargetAttackerHandler, MainTargetAttackerHandler.Factory>()
                .FromSubContainerResolve()
                .ByNewContextPrefab(_recourses.Prefabs.MainTargetAttacker);

            Container.BindFactory<TurretEntity, TurretEntity.Factory>()
                .WithId(TurretType.VoiceOfTruth)
                .FromSubContainerResolve()
                .ByNewContextPrefab(_recourses.Prefabs.VoiceOfTruthTurret);
        }
    }
}