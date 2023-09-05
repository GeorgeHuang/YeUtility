
namespace CommonUnit
{
    public class FieldData : UnityEngine.ScriptableObject
    {
        public string Name;
        public void Init() => Name = name;
        virtual public System.Object InitValue() { return null; }
        virtual public System.Object getValue() { return null; }
        virtual public System.Object getValueWithLv(int lv) { return null; }
        virtual public System.Object getCostWithLv(int lv) { return null; }
        virtual public System.Object getMaxLV() { return 1; }
    }
}
