using UnityEngine;

namespace YeUtility
{
    public class DontDestroy : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            DontDestroyOnLoad(this);
        }
    }
}
