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
            var damage = Mathf.RoundToInt(runtimeSkill.Damage);
            var lv = runtimeSkill.Lv;
            textMeshProUGUI.text = $"{runtimeSkill.GetDisplayName()}_Lv{lv}: {damage}";
        }
    }
}