namespace YeUtility
{
    public class RecoverValueFloat
    {
        private float recorverValue;

        public RecoverValueFloat(RecoverValueFloat recoverValueFloat)
        {
            CurValue = recoverValueFloat.CurValue;
            MaxValue = recoverValueFloat.MaxValue;
            recorverValue = recoverValueFloat.recorverValue;
            IsFull = recoverValueFloat.IsFull;
        }

        public RecoverValueFloat(SettingData setting)
        {
            Setup(setting);
            CurValue = MaxValue;
            IsFull = true;
        }

        public float CurValue { get; private set; }

        public float MaxValue { get; private set; }

        public float Ratio => CurValue / MaxValue;

        public bool IsFull { get; private set; }

        public void Tick(float deltaTime)
        {
            if (IsFull) return;
            Add(recorverValue * deltaTime);
        }

        public void Add(float value)
        {
            CurValue += value;
            if (!(CurValue > MaxValue)) return;
            CurValue = MaxValue;
            IsFull = true;
        }

        public void Reduce(float value)
        {
            CurValue -= value;
            if (CurValue < 0) CurValue = 0;
        }

        public RecoverValueFloat Clear()
        {
            CurValue = 0;
            IsFull = false;
            return this;
        }


        public void Setup(SettingData setting)
        {
            MaxValue = setting.Max;
            recorverValue = setting.Recover;
        }

        public static RecoverValueFloat operator +(RecoverValueFloat left, float right)
        {
            var rv = new RecoverValueFloat(left);
            rv.CurValue += right;
            rv.IsFull = rv.CurValue >= left.MaxValue;
            if (rv.IsFull) rv.CurValue = left.MaxValue;
            return rv;
        }

        public struct SettingData
        {
            public float Max;
            public float Recover;
        }
    }
}