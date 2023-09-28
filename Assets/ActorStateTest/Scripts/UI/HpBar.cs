using System;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace ActorStateTest.Scripts.UI
{
    public class HpBar : MonoBehaviour
    {
        [SerializeField] private Image midImage;
        [SerializeField] private Image frontImage;
        [SerializeField] private float midDelay = 1;

        [Button("SerPercent")]
        public void SetPercent(float percent)
        {
            frontImage.fillAmount = percent;
            
            if (DelayWorking == false)
            {
                DelayMid().Forget();
            }
        }

        private bool DelayWorking { get; set; }

        private async UniTaskVoid DelayMid()
        {
            DelayWorking = true;
            await UniTask.WaitForSeconds(midDelay);
            midImage.fillAmount = frontImage.fillAmount;
            DelayWorking = false;
        }
    }
}