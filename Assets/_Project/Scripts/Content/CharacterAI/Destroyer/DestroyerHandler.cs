using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Project.Content.CharacterAI.Destroyer
{
    public class DestroyerHandler : CharacterHandler
    {
        [SerializeField] private DestroyerData _destroyerData;
        private bool _canAttack;
        private bool _canMoving;
        private CharacterSensor _characterSensor;

        public ICharacterData DestroyerData => _destroyerData;

        public bool CanAttack => _canAttack;
        public bool CanMoving => _canMoving;

        [Inject]
        public void Construct(CharacterHealthHandler healthHandler, EnemyDeadHandler enemyDeadHandler, CharacterSensor characterSensor)
        {
            _destroyerData.ThisEntity = this;
            _healthHandler = healthHandler;
            _enemyDeadHandler = enemyDeadHandler;
            _characterSensor = characterSensor;

            _characterSensor.TargetDetected += HasTarget;
            _characterSensor.HasTargetToAttack += HasTargetToAttack;
            _cancellationToken = this.GetCancellationTokenOnDestroy();
        }

        public override T ProvideComponent<T>() where T : class
        {
            if (_destroyerData.Flags is T flags)
                return flags;

            if (_healthHandler is T healthHandler)
                return healthHandler;

            if (transform is T characterTransform)
                return characterTransform;

            if (_enemyDeadHandler is T deadHandler)
                return deadHandler;

            return null;
        }

        private void Start()
        {
            _healthHandler.Initialize();
        }

        private void Update()
        {
            LookAt();

        }

        private void LookAt()
        {
            if (_characterSensor.TargetTransformToChase == null)
                return;

            Vector3 direction = _characterSensor.TargetTransformToChase.position - _destroyerData.CharacterTransform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        private void HasTarget()
        {
            _canMoving = true;
        }
        
        private void HasTargetToAttack()
        {
            _canAttack = true;
        }

        private void OnDestroy()
        {
            _characterSensor.TargetDetected -= HasTarget;
            _characterSensor.HasTargetToAttack -= HasTargetToAttack;
        }
    }
}

