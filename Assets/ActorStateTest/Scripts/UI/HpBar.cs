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

        [Button("SerPercent")]
        public void SetPercent(float percent)
        {
            frontImage.fillAmount = percent;
            if (Math.Abs(frontImage.fillAmount - midImage.fillAmount) > 0.001f)
            {
                DelayMid().Forget();
            }
        }

        async UniTaskVoid DelayMid()
        {
            await UniTask.WaitForSeconds(1);
            midImage.fillAmount = frontImage.fillAmount;
        }
    }
}