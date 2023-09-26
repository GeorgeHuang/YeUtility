using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YeActorState.RuntimeCore;

namespace YeActorState.UI
{
    public class SkillObjectElement : MonoBehaviour
    {
        public Button Btn;
        [SerializeField] private TextMeshProUGUI nameText;
        public void Setup(SkillObject skillObject)
        {
            nameText.text = skillObject.GetDisplayName();
        }
    }
}