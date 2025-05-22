using Project.Content.CharacterAI;
using Project.Content.CharacterAI.Infantryman;
using Project.Content.ObjectPool;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Project.Content.BuildSystem
{
    public class BarracksSpawnLogic : ITickable
    {
        private BarracksEntity _barracksEntity;
        private EntityCommander _entityCommander;
        private PauseHandler _pauseHandler;
        private int _currentCountAlly = 0;
        private float _attackCooldownTimer;
        private List<GameObject> _allyAlive = new List<GameObject>();
        private bool _isActive;
        private AllyEntityType _infantrymanData => AllyEntityType.Infantryman;
        private FiltrablePoolsHandler _poolsHandler;

        private Predicate<InfantrymanEntity> InfantrymanPredicate
            => item => item.Type == _infantrymanData && item.gameObject.activeInHierarchy == false;

        public BarracksSpawnLogic(BarracksEntity barracksEntity,
                                  EntityCommander entityCommander,
                                  PauseHandler pauseHandler,
                                  FiltrablePoolsHandler poolsHandler = null)
        {
            _barracksEntity = barracksEntity;
            _entityCommander = entityCommander;
            _pauseHandler = pauseHandler;
            _poolsHandler = poolsHandler;
        }

        public void Initialize()
        {
            _attackCooldownTimer = _barracksEntity.Data.SpawnCooldown;


            _isActive = true;
        }

        public void Tick()
        {
            if (!_isActive)
                return;

            if (_pauseHandler.IsPaused)
                return;

            if (_currentCountAlly < _barracksEntity.Data.Capacity)
            {
                if (_attackCooldownTimer <= 0)
                {
                    Spawn();
                }
            }

            CheckOnAlive();

            CooldownSpawn();

        }

        private void CheckOnAlive()
        {
            if (_currentCountAlly == 0)
                return;

            for (int i = 0; i < _allyAlive.Count; i++)
            {
                if (!_allyAlive[i].activeSelf)
                {
                    var entity = _allyAlive[i].GetComponent<IPatrolling>();
                    _entityCommander.RemoveEntity(entity);
                    _allyAlive.RemoveAt(i);
                    _currentCountAlly--;
                }
            }
        }

        private void CooldownSpawn()
        { 
            if (_attackCooldownTimer > 0)
            {
                _attackCooldownTimer -= Time.deltaTime;
            }
        }

        private void Spawn()
        {
            _attackCooldownTimer = _barracksEntity.Data.SpawnCooldown;

            var infantryman = _poolsHandler.GetByPredicate<InfantrymanEntity>(InfantrymanPredicate, _barracksEntity.Data.SpawnPosition);

            _allyAlive.Add(infantryman.gameObject);

            infantryman.Initialize();
            PrepareInfantryman(infantryman);

            _currentCountAlly++;
        }

        private void PrepareInfantryman(InfantrymanEntity infantryman)
        {
            var patrolling = infantryman.ProvideComponent<IPatrolling>();

            _entityCommander.AddEntity(patrolling);

            infantryman.Prepare(new InfantrymanSpawnData(_barracksEntity.Data.UnitDamageModifier, _barracksEntity.Data.UnitHealthModifier, _barracksEntity.Data.UnitUpgradeCount));
        }
    }
}