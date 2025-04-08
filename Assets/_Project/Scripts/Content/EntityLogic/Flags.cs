using System;
using System.Linq;
using UnityEngine;

namespace Project.Content
{
    [Serializable]
    public sealed class Flags
    {
        [SerializeField] private EntityFlags[] _flags;

        public EntityFlags[] Values => _flags;

        public bool Contain(EntityFlags flag)
            => _flags.Contains(flag);

        public bool Contain(EntityFlags[] flags)
            => flags.Any(flag => _flags.Contains(flag));
    }
}