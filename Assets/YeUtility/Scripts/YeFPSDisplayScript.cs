using TMPro;
using UnityEngine;

namespace YeUtility
{
    public class YeFPSDisplayScript : MonoBehaviour
    {
        public int fps;
        public int lastFPS;
        public GUIStyle textStyle;
        public TextMeshProUGUI textMesh;
        private bool hasMesh;
        private float timeA;

        // Use this for initialization
        private void Start()
        {
            timeA = Time.realtimeSinceStartup;
            textStyle.fontSize = 60;
            //DontDestroyOnLoad(this);
            hasMesh = textMesh != null;
        }

        // Update is called once per frame
        private void Update()
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

        private void OnGUI()
        {
            if (hasMesh) return;
            GUI.Label(new Rect(5, 5, 300, 300), "" + lastFPS, textStyle);
        }
    }
}