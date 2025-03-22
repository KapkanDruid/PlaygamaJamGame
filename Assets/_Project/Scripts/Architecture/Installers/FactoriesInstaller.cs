using Project.Content;
using Project.Content.BuildSystem;
using Project.Content.CharacterAI;
using Project.Content.CharacterAI.Destroyer;
using Project.Content.CharacterAI.MainTargetAttacker;
using Project.Content.Spawners;
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
                .WithFactoryArguments(DestroyerType.SimpleParanoid)
                .FromSubContainerResolve()
                .ByNewContextPrefab(_recourses.Prefabs.SimpleParanoid);
            
            Container.BindFactory<DestroyerHandler, DestroyerHandler.Factory>()
                .WithFactoryArguments(DestroyerType.AdvencedParanoid)
                .FromSubContainerResolve()
                .ByNewContextPrefab(_recourses.Prefabs.AdvencedParanoid);            

            Container.BindFactory<DestroyerHandler, DestroyerHandler.Factory>()
                .WithFactoryArguments(DestroyerType.Aliens)
                .FromSubContainerResolve()
                .ByNewContextPrefab(_recourses.Prefabs.Aliens); 

            Container.BindFactory<DestroyerHandler, DestroyerHandler.Factory>()
                .WithFactoryArguments(DestroyerType.FlatEarther)
                .FromSubContainerResolve()
                .ByNewContextPrefab(_recourses.Prefabs.FlatEarther);

            Container.BindFactory<MainTargetAttackerHandler, MainTargetAttackerHandler.Factory>()
                .WithFactoryArguments(MainTargetAttackerType.Bigfoot)
                .FromSubContainerResolve()
                .ByNewContextPrefab(_recourses.Prefabs.BigFoot);

            Container.BindFactory<MainTargetAttackerHandler, MainTargetAttackerHandler.Factory>()
                .WithFactoryArguments(MainTargetAttackerType.HumanMoth)
                .FromSubContainerResolve()
                .ByNewContextPrefab(_recourses.Prefabs.HumanMoth);

            Container.BindFactory<DestroyerType, DestroyerSpawner, DestroyerSpawner.Factory>();
            Container.BindFactory<MainTargetAttackerType, MainTargetAttackerSpawner, MainTargetAttackerSpawner.Factory>();

            Container.BindFactory<TurretEntity, TurretEntity.Factory>()
                .WithId(TurretType.VoiceOfTruth)
                .FromSubContainerResolve()
                .ByNewContextPrefab(_recourses.Prefabs.VoiceOfTruthTurret);
        }
    }
}