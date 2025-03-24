using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Project.Content.CharacterAI.Infantryman
{
    public class InfantrymanInstaller : MonoInstaller 
    {
        [SerializeField] private InfantrymanEntity _entity;
        [SerializeField] private GizmosDrawer _gizmosDrawer;
        [SerializeField] private Animator _animator;
        [SerializeField] private AnimatorEventHandler _animatorEventHandler;

        public override void InstallBindings()
        {
            Container.Bind<Animator>().FromInstance(_animator).AsSingle().NonLazy();
            Container.Bind<GizmosDrawer>().FromInstance(_gizmosDrawer).AsSingle().NonLazy();
            Container.Bind<NavMeshAgent>().FromComponentOnRoot().AsSingle().NonLazy();
            Container.Bind<AnimatorEventHandler>().FromInstance(_animatorEventHandler).AsSingle();
            Container.BindInterfacesAndSelfTo<InfantrymanAttackLogic>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PatrolLogic>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<InfantrymanMoveLogic>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<InfantrymanEntity>().FromInstance(_entity).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<InfantrymanData>().FromInstance(_entity.InfantrymanData).AsSingle().NonLazy();
            Container.Bind<CharacterHealthHandler>().AsSingle().WithArguments(_entity.InfantrymanData.Health).NonLazy();
            Container.Bind<EnemyDeadHandler>().AsSingle().WithArguments(_entity.transform).NonLazy();

        }
    }
}

