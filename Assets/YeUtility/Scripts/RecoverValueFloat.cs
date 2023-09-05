
namespace CommonUnit
{
    public class RecoverValueFloat
    {
        float curValue;
        float maxValue;
        float recorverValue;

        bool isFull = false;

        public RecoverValueFloat(RecoverValueFloat recoverValueFloat)
        {
            curValue= recoverValueFloat.curValue;
            maxValue= recoverValueFloat.maxValue;
            recorverValue = recoverValueFloat.recorverValue;
            isFull = recoverValueFloat.isFull;
        }

        public RecoverValueFloat(SettingData setting)
        {
            Setup(setting);
            curValue = maxValue;
            isFull = true;
        }

        public float CurValue => curValue;
        public float MaxValue => maxValue;
        public float Ratio => curValue / maxValue;

        public bool IsFull => isFull;

        public void Tick(float deltaTime)
        {
            if (isFull) { return; }
            Add(recorverValue * deltaTime);
        }

        public void Add(float value)
        {
            curValue += value;
            if (curValue > maxValue)
            {
                curValue = maxValue; 
                isFull= true;
            }
        }

        public void Reduce(float value)
        {
            curValue -= value;
            if (curValue < 0) { curValue = 0; }
        }

        public RecoverValueFloat Clear()
        { 
            curValue = 0;
            isFull = false;
            return this;
        }
            

        public void Setup(SettingData setting)
        {
            maxValue = setting.Max;
            recorverValue = setting.Recover;
        }

        public static RecoverValueFloat operator +(RecoverValueFloat left, float right)
        {
            var rv = new RecoverValueFloat(left);
            rv.curValue += right;
            rv.isFull = rv.curValue >= left.maxValue;
            if (rv.isFull ) { rv.curValue = left.maxValue; }
            return rv;
        }

        public struct SettingData
        {
            public float Max;
            public float Recover;
        }
    }
}