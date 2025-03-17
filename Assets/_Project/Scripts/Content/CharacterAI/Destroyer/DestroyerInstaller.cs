using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Project.Content.CharacterAI.Destroyer.Destroyer
{
    public class DestroyerInstaller : MonoInstaller
    {
        [SerializeField] private DestroyerHandler _destroyerHandler;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Animator _animator;
        [SerializeField] private AnimatorEventHandler _animatorEventHandler;

        public override void InstallBindings()
        {
            Container.Bind<CharacterHealthHandler>().AsSingle().NonLazy();
            Container.Bind<Animator>().FromInstance(_animator).AsSingle().NonLazy();
            Container.Bind<EnemyDeadHandler>().AsSingle().WithArguments(_destroyerHandler.DestroyerData).NonLazy();
            Container.Bind<DestroyerHandler>().FromComponentOnRoot().AsSingle().NonLazy();
            Container.Bind<NavMeshAgent>().FromComponentOnRoot().AsSingle().NonLazy();
            Container.Bind<ICharacterData>().FromInstance(_destroyerHandler.DestroyerData).AsSingle();
            Container.Bind<ISensorData>().FromInstance((ISensorData)_destroyerHandler.DestroyerData).AsSingle();
            Container.Bind<Rigidbody2D>().FromInstance(_rigidbody).AsSingle();
            Container.Bind<AnimatorEventHandler>().FromInstance(_animatorEventHandler).AsSingle();
            Container.BindInterfacesAndSelfTo<CharacterSensor>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<DestroyerMoveLogic>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<DestroyerAttackLogic>().AsSingle().NonLazy();
        }
    }
}