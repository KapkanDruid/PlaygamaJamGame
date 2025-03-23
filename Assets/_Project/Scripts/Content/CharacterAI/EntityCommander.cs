using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Content.CharacterAI
{
    public class EntityCommander 
    {
        private List<Transform> _defensiveFlagsTransforms;
        private List<DefensiveFlag> _defensiveFlags;
        private List<IPatrolling> _entity;

        public void Initialize()
        {
            _defensiveFlags = new List<DefensiveFlag>();
            _entity = new List<IPatrolling>();
            TryGetFlags();
            RedistributeEntities();
        }

        public void AddEntity(IPatrolling entity)
        {
            _entity.Add(entity);
            AssignEntityToFlag(entity);
        }

        public void RemoveEntity(IPatrolling entity)
        {
            var flag = entity.FlagTransform.GetComponent<DefensiveFlag>();
            if (flag != null)
            {
                flag.RemoveUnit();
            }
            _entity.Remove(entity);
        }

        public void AddFlag(DefensiveFlag flag)
        {
            _defensiveFlags.Add(flag);
            RedistributeEntities();
        }

        public void RemoveFlag(DefensiveFlag flag)
        {
            _defensiveFlags.Remove(flag);
            RedistributeEntities();
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

        private void AssignEntityToFlag(IPatrolling entity)
        {
            DefensiveFlag leastFullFlag = _defensiveFlags.OrderBy(f => f.Fullness).FirstOrDefault();
            if (leastFullFlag != null)
            {
                entity.SetFlag(leastFullFlag.transform);
                leastFullFlag.AddUnit();
            }
        }

        private void RedistributeEntities()
        {
            foreach (var flag in _defensiveFlags)
            {
                flag.RemoveUnit();
            }

            foreach (var entity in _entity)
            {
                AssignEntityToFlag(entity);
            }
        }

    }
}