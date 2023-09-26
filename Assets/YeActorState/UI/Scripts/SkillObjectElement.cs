using Microsoft.Cci;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YeActorState.RuntimeCore;
using YeUtility;

namespace YeActorState.UI
{
    public class SkillObjectElement : MonoBehaviour
    {
        public Button Btn;
        [SerializeField] private TextMeshProUGUI nameText;
        public void Setup(INamedObject skillObject)
        {
            nameText.text = skillObject.GetDisplayName();
        }
    }
}