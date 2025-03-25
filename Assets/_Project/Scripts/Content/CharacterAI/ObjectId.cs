using UnityEngine;

namespace Project.Content.CharacterAI
{
    public class ObjectId : MonoBehaviour
    {
        [SerializeField] private int _objectId;

        public int Id => _objectId;
    }
}
