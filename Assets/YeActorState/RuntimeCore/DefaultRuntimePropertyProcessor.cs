namespace YeActorState.RuntimeCore
{
    public class DefaultRuntimePropertyProcessor : IBasePropertyProcessor
    {
        public void Processor(YeActorBaseData baseData, YeActorRuntimeData runtimeData)
        {
            ApplyRatioValue(runtimeData, "MoveSpeed", "MoveSpeedRatio");
            ApplyRatioValue(runtimeData, "Damage", "DamageRatio");
            ApplyRatioValue(runtimeData, "AtkSpeed", "AtkSpeedRatio");
        }

        private static void ApplyRatioValue(YeActorRuntimeData runtimeData, string key, string ratioKey)
        {
            var value = runtimeData.GetProperty(key);
            var valueRatio = runtimeData.GetProperty(ratioKey);
            value *= 1 + valueRatio * 0.01f;
            runtimeData.SetProperty(key, value);
        }
    }
}