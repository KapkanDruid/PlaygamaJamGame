using System.Collections.Generic;
using UnityEngine;

namespace Project.Content.Spawners
{
    public class SpawnPointHintHandler : MonoBehaviour
    {
        [SerializeField] private EnemyWaveSpawner _enemyWaveSpawner;
        [SerializeField] private List<GameObject> _hintObjects;
        [SerializeField] private float _animationSpeed;
        [SerializeField] private int _timeAnimation;

        private void OnEnable()
        {
            _enemyWaveSpawner.OnSpawnPointSelected += HandleSpawnPointSelected;
        }

        private void OnDisable()
        {
            _enemyWaveSpawner.OnSpawnPointSelected -= HandleSpawnPointSelected;
        }

        private void HandleSpawnPointSelected(Transform spawnPoint)
        {
            GameObject closestHintObject = null;
            float closestDistance = float.MaxValue;

            foreach (var hintObject in _hintObjects)
            {
                float distance = Vector3.Distance(spawnPoint.position, hintObject.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestHintObject = hintObject;
                }
            }

            if (closestHintObject != null)
            {
                closestHintObject.SetActive(true);
                var hintAnimatorController = closestHintObject.GetComponent<HintAnimatorController>();

                if (hintAnimatorController != null)
                {
                    hintAnimatorController.PlayAnimation(_timeAnimation, _animationSpeed);
                }
            }
        }
    }
}
