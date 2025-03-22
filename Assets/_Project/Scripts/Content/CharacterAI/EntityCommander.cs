using System.Collections.Generic;
using UnityEngine;

namespace Project.Content.CharacterAI
{
    public class EntityCommander : MonoBehaviour
    {
        [SerializeField] private List<Transform> _defensiveFlagsTransforms;

        private List<DefensiveFlag> _defensiveFlags;

        private void Awake()
        {
            TryGetFlags();
        }

        public void OrderMoveToFlag()
        {
            if (_defensiveFlags.Count == 0)
            {
                return;
            }
        }

        public void AddFlag(DefensiveFlag flag)
        {
            _defensiveFlags.Add(flag);
        }

        public void RemoveFlag(DefensiveFlag flag)
        {
            _defensiveFlags.Remove(flag);
        }

        private void TryGetFlags()
        {
            for (int i = 0; i < _defensiveFlagsTransforms.Count; i++)
            {
                Transform flagTransform = _defensiveFlagsTransforms[i];
                DefensiveFlag flag;
                if (flagTransform.TryGetComponent<DefensiveFlag>(out flag))
                {
                    _defensiveFlags.Add(flag);
                }
                else
                {
                    return;
                }

            }
        }

    }
}