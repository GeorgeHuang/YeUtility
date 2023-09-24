using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using YeActorState.RuntimeCore;

namespace YeActorState.UI
{
    public class PropertyEffectElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI labelText;

        private PropertyEffectData propertyEffectData;

        public void Setup(PropertyEffectData propertyEffectData)
        {
            this.propertyEffectData = propertyEffectData;
            labelText.text = propertyEffectData.GetDisplayName();
        }
    }
}