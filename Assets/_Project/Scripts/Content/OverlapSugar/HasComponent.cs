using UnityEngine;

namespace Project.Content.OverlapSugar
{
    internal delegate bool HasComponent<T>(Component component, out T target);
}