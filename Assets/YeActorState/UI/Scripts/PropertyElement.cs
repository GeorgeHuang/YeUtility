using TMPro;
using UnityEngine;

namespace YeActorState.UI
{
    public class PropertyElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI propertyNameText;
        
        private ActorStateHandler stateHandler;

        public void Setup(string propertyName, ActorStateHandler stateHandler)
        {
            this.stateHandler = stateHandler;
            propertyNameText.text = propertyName;
        }
    }
}