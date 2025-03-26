using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Project.Content
{
    public sealed class GizmosDrawer : MonoBehaviour
    {
        private List<IGizmosDrawer> _gizmosDrawers = new();
        private List<IGizmosDrawerOnSelected> _gizmosDrawerOnSelected = new();

        [Inject]
        private void Construct(List<IGizmosDrawer> gizmosDrawers, List<IGizmosDrawerOnSelected> gizmosDrawerOnSelected)
        {
            _gizmosDrawers.AddRange(gizmosDrawers);
            _gizmosDrawerOnSelected.AddRange(gizmosDrawerOnSelected);
        }

        public void AddGizmosDrawer(IGizmosDrawer gizmosDrawer)
        {
            _gizmosDrawers.Add(gizmosDrawer);
        }

        public void AddGizmosDrawer(IGizmosDrawerOnSelected gizmosDrawer)
        {
            _gizmosDrawerOnSelected.Add(gizmosDrawer);
        }

        private void OnDrawGizmos()
        {
            if (_gizmosDrawers == null)
                return;

            for (int i = 0; i < _gizmosDrawers.Count; i++)
            {
                IGizmosDrawer drawer = _gizmosDrawers[i];
                drawer.OnDrawGizmos();
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (_gizmosDrawerOnSelected == null)
                return;

            for (int i = 0; i < _gizmosDrawerOnSelected.Count; i++)
            {
                IGizmosDrawerOnSelected drawer = _gizmosDrawerOnSelected[i];
                drawer.OnDrawGizmosSelected();
            }
        }
    }
}
