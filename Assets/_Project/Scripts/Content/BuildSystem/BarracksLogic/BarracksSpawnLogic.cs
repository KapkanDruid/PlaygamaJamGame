using Project.Content.CharacterAI;
using Project.Content.Spawners;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Project.Content.BuildSystem
{
    public class BarracksSpawnLogic : ITickable
    {
        private BarracksEntity _barracksEntity;
        private EntityCommander _entityCommander;
        private AlliedRangerSpawner.Factory _alliedRangerFactory;
        private PauseHandler _pauseHandler;
        private IEnemySpawner _alliedRangerSpawner;
        private int _currentCountAlly = 0;
        private float _attackCooldownTimer;
        private List<GameObject> _allyAlive = new List<GameObject>();

        public BarracksSpawnLogic(BarracksEntity barracksEntity,
                                  EntityCommander entityCommander,
                                  AlliedRangerSpawner.Factory alliedRangerFactory,
                                  PauseHandler pauseHandler)
        {
            _barracksEntity = barracksEntity;
            _entityCommander = entityCommander;
            _alliedRangerFactory = alliedRangerFactory;
            _pauseHandler = pauseHandler;

            _entityCommander.Initialize();

            _alliedRangerSpawner = _alliedRangerFactory.Create(_barracksEntity.Data.AllyEntityType);
            _attackCooldownTimer = _barracksEntity.Data.SpawnCooldown;

            _alliedRangerSpawner.Initialize(_barracksEntity.Data.Capacity);
        }

        public void Tick()
        {
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
            _alliedRangerSpawner.Spawn(_barracksEntity.transform.position);
            _attackCooldownTimer = _barracksEntity.Data.SpawnCooldown;

            var obj = _alliedRangerSpawner.Prefab;
            _allyAlive.Add(obj);
            var entity = obj.GetComponent<IPatrolling>();

            _entityCommander.AddEntity(entity);

            _currentCountAlly++;
        }
    }
}