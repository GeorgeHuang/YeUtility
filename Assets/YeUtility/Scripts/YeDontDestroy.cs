using UnityEngine;

namespace YeUtility
{
    public class YeDontDestroy : MonoBehaviour
    {
        // Use this for initialization
        private void Start()
        {
            DontDestroyOnLoad(this);
        }
    }
}