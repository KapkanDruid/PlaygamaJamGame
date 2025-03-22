using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Project.Content.CharacterAI.Destroyer
{
    public class DestroyerInstaller : MonoInstaller
    {
        [SerializeField] private DestroyerHandler _destroyerHandler;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private GizmosDrawer _gizmosDrawer;
        [SerializeField] private Animator _animator;
        [SerializeField] private AnimatorEventHandler _animatorEventHandler;

        public override void InstallBindings()
        {
            Container.Bind<Animator>().FromInstance(_animator).AsSingle().NonLazy();
            Container.Bind<GizmosDrawer>().FromInstance(_gizmosDrawer).AsSingle().NonLazy();
            Container.Bind<EnemyDeadHandler>().AsSingle().WithArguments(_destroyerHandler.transform).NonLazy();
            Container.Bind<DestroyerHandler>().FromInstance(_destroyerHandler).AsSingle().NonLazy();
            Container.Bind<NavMeshAgent>().FromComponentOnRoot().AsSingle().NonLazy();
            Container.Bind<Rigidbody2D>().FromInstance(_rigidbody).AsSingle();
            Container.Bind<AnimatorEventHandler>().FromInstance(_animatorEventHandler).AsSingle();
            Container.BindInterfacesAndSelfTo<DestroyerData>().FromInstance(_destroyerHandler.DestroyerData).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<CharacterSensor>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<DestroyerMoveLogic>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<DestroyerAttackLogic>().AsSingle().NonLazy();
        }
    }
}