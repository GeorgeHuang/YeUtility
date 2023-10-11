using UnityEngine;

namespace YeUtility
{
    public class YeDontDestroy : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            DontDestroyOnLoad(this);
        }
    }
}
