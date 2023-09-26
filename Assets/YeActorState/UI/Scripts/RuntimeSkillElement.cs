using TMPro;
using UnityEngine;
using YeActorState.RuntimeCore;

namespace YeActorState.UI
{
    internal class RuntimeSkillElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textMeshProUGUI;

        public void Setup(RuntimeSkill runtimeSkill)
        {
            textMeshProUGUI.text = runtimeSkill.GetDisplayName() + " " + runtimeSkill.Damage;
        }
    }
}