
namespace YeUtility
{
    public class FieldData : UnityEngine.ScriptableObject
    {
        public string Name;
        public void Init() => Name = name;
        virtual public System.Object InitValue() { return null; }
        virtual public System.Object GetValue() { return null; }
        virtual public System.Object GetValueWithLv(int lv) { return null; }
        virtual public System.Object GetCostWithLv(int lv) { return null; }
        virtual public System.Object GetMaxLv() { return 1; }
    }
}
