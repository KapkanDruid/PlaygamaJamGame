using UnityEngine;


namespace Project.Content
{
    public class OpenFidLink : MonoBehaviour
    {
        [SerializeField] private string _link;

        public void OpenLink()
        {
            Application.OpenURL(_link);
        }
        
    }
}
