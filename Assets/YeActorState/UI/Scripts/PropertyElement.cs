using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace YeActorState.UI
{
    public class PropertyElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI propertyNameText;
        [SerializeField] private Image image;
        [SerializeField] private List<Color> colos;

        [Inject] private PropertyNames propertyNames;
        
        private ActorStateHandler stateHandler;
        private string propertyName;
        private int index;

        public void Setup(string propertyName, ActorStateHandler stateHandler, int index)
        {
            this.stateHandler = stateHandler;
            this.propertyName = propertyName;
            var displayName = propertyNames.GetDisplayName(propertyName);
            propertyNameText.text = displayName;
            this.index = index;
            image.color = colos[index % 2];
        }
    }
}