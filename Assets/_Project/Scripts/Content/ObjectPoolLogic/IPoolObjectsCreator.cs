using System.Collections.Generic;
using UnityEngine;

namespace Project.Content
{
    public interface IPoolObjectsCreator<T> where T : MonoBehaviour
    {
        public List<T> InstantiateObjects(int count, GameObject parentObject);
        public T Instantiate();
    }
}
