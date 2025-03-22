using UnityEngine;

namespace Project.Content.BuildSystem
{
    public interface ISensorFilter
    {
        public bool TryToFilter(RaycastHit2D[] hits, out IEntity foundedEntity, out Transform targetTransform);
    }
}
