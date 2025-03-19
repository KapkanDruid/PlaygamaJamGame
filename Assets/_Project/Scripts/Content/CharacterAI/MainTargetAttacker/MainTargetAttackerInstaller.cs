using UnityEngine.AI;
using UnityEngine;
using Zenject;

namespace Project.Content.CharacterAI.MainTargetAttacker
{
    class MainTargetAttackerInstaller : MonoInstaller
    {
        [SerializeField] private MainTargetAttackerHandler _mainTargetAttackerHandler;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private GizmosDrawer _gizmosDrawer;
        [SerializeField] private Animator _animator;
        [SerializeField] private AnimatorEventHandler _animatorEventHandler;

        public override void InstallBindings()
        {
            Container.Bind<CharacterHealthHandler>().AsSingle().NonLazy();
            Container.Bind<Animator>().FromInstance(_animator).AsSingle().NonLazy();
            Container.Bind<EnemyDeadHandler>().AsSingle().WithArguments((ISensorData)_mainTargetAttackerHandler.MainTargetAttackerData).NonLazy();
            Container.Bind<MainTargetAttackerHandler>().FromComponentOnRoot().AsSingle().NonLazy();
            Container.Bind<NavMeshAgent>().FromComponentOnRoot().AsSingle().NonLazy();
            Container.Bind<Rigidbody2D>().FromInstance(_rigidbody).AsSingle();
            Container.Bind<GizmosDrawer>().FromInstance(_gizmosDrawer).AsSingle().NonLazy();
            Container.Bind<AnimatorEventHandler>().FromInstance(_animatorEventHandler).AsSingle();
            Container.BindInterfacesAndSelfTo<MainTargetAttackerData>().FromInstance(_mainTargetAttackerHandler.MainTargetAttackerData).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<CharacterSensor>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<MainTargetAttackerMoveLogic>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<MainTargetAttackerAttackLogic>().AsSingle().NonLazy();
        }
    }
}
