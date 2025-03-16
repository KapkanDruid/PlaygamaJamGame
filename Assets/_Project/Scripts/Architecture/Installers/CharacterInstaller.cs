using Content;
using Content.Character;
using UnityEngine;
using Zenject;

namespace Architecture.Installers
{
    public class CharacterInstaller : MonoInstaller
    {
        [SerializeField] private CharacterHandler _characterHandler;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Animator _animator;
        [SerializeField] private AnimatorEventHandler _animatorEventHandler;

        public override void InstallBindings()
        {
            Container.Bind<CharacterHealthHandler>().AsSingle().NonLazy();
            Container.Bind<Animator>().FromInstance(_animator).AsSingle().NonLazy();
            Container.Bind<EnemyDeadHandler>().AsSingle().NonLazy();
            Container.Bind<CharacterHandler>().FromComponentOnRoot().AsSingle().NonLazy();
            Container.Bind<CharacterData>().FromInstance(_characterHandler.CharacterData).AsSingle();
            Container.Bind<Rigidbody2D>().FromInstance(_rigidbody).AsSingle();
            Container.Bind<AnimatorEventHandler>().FromInstance(_animatorEventHandler).AsSingle();
            Container.BindInterfacesAndSelfTo<CharacterSensor>().AsSingle().NonLazy();
        }
    }
}