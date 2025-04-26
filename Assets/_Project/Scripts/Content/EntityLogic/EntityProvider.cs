using UnityEngine;

namespace Project.Content
{
    public class EntityProvider : MonoBehaviour, IEntity
    {
        [SerializeField] private GameObject _entityReference;

        private IEntity _entity;

        private void Start()
        {
            _entity = _entityReference.GetComponent<IEntity>();
        }

        public T ProvideComponent<T>() where T : class
        {
            if (_entity != null)
                return _entity.ProvideComponent<T>();
            else 
                return null;
        }
    }
}
