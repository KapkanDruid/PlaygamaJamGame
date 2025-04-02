using Project.Content;
using Project.Content.BuildSystem;
using Project.Content.CharacterAI;
using Project.Content.CharacterAI.Destroyer;
using Project.Content.CharacterAI.Infantryman;
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

            Container.BindFactory<DestroyerEntity, DestroyerEntity.Factory>()
                .WithFactoryArguments(DestroyerType.SimpleParanoid)
                .FromSubContainerResolve()
                .ByNewContextPrefab(_recourses.Prefabs.SimpleParanoid);

            Container.BindFactory<DestroyerEntity, DestroyerEntity.Factory>()
                .WithFactoryArguments(DestroyerType.AdvencedParanoid)
                .FromSubContainerResolve()
                .ByNewContextPrefab(_recourses.Prefabs.AdvencedParanoid);

            Container.BindFactory<DestroyerEntity, DestroyerEntity.Factory>()
                .WithFactoryArguments(DestroyerType.Aliens)
                .FromSubContainerResolve()
                .ByNewContextPrefab(_recourses.Prefabs.Aliens);

            Container.BindFactory<DestroyerEntity, DestroyerEntity.Factory>()
                .WithFactoryArguments(DestroyerType.FlatEarther)
                .FromSubContainerResolve()
                .ByNewContextPrefab(_recourses.Prefabs.FlatEarther);

            Container.BindFactory<InfantrymanEntity, InfantrymanEntity.Factory>()
                .WithFactoryArguments(AllyEntityType.Infantryman)
                .FromSubContainerResolve()
                .ByNewContextPrefab(_recourses.Prefabs.Infantryman);

            Container.BindFactory<MainTargetAttackerEntity, MainTargetAttackerEntity.Factory>()
                .WithFactoryArguments(MainTargetAttackerType.Bigfoot)
                .FromSubContainerResolve()
                .ByNewContextPrefab(_recourses.Prefabs.BigFoot);

            Container.BindFactory<MainTargetAttackerEntity, MainTargetAttackerEntity.Factory>()
                .WithFactoryArguments(MainTargetAttackerType.HumanMoth)
                .FromSubContainerResolve()
                .ByNewContextPrefab(_recourses.Prefabs.HumanMoth);

            Container.BindFactory<DestroyerType, DestroyerSpawner, DestroyerSpawner.Factory>();
            Container.BindFactory<MainTargetAttackerType, MainTargetAttackerSpawner, MainTargetAttackerSpawner.Factory>();
            Container.BindFactory<AllyEntityType, AlliedRangerSpawner, AlliedRangerSpawner.Factory>();

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