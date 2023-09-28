using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace ActorStateTest.Scripts.UI
{
    public class HpBar : MonoBehaviour
    {
        [SerializeField] private Image midImage;
        [SerializeField] private Image frontImage;

        [Button("Serpercent")]
        void SetPercent(float percent)
        {
            
        }
    }
}