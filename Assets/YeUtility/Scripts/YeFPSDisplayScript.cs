using TMPro;
using UnityEngine;

namespace YeUtility
{
    public class YeFPSDisplayScript : MonoBehaviour
    {
        bool hasMesh = false;
        float timeA;
        public int fps;
        public int lastFPS;
        public GUIStyle textStyle;
        public TextMeshProUGUI textMesh;

        // Use this for initialization
        void Start()
        {
            timeA = Time.realtimeSinceStartup;
            textStyle.fontSize = 60;
            //DontDestroyOnLoad(this);
            hasMesh = textMesh != null;
        }

        // Update is called once per frame
        void Update()
        {
            //Debug.Log(Time.timeSinceLevelLoad+" "+timeA);
            if (Time.realtimeSinceStartup - timeA <= 1)
            {
                fps++;
            }
            else
            {
                lastFPS = fps + 1;
                timeA = Time.realtimeSinceStartup;
                fps = 0;
            }

            if (hasMesh) textMesh.text = lastFPS.ToString();
        }
        void OnGUI()
        {
            if (hasMesh) return;
            GUI.Label(new Rect(5, 5, 300, 300), "" + lastFPS, textStyle);
        }
    }
}
