using UnityEngine;

namespace CommonUnit
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
