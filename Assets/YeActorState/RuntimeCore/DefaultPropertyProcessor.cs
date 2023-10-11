namespace YeActorState.RuntimeCore
{
    public class DefaultPropertyProcessor : IBasePropertyProcessor
    {
        public void Processor(YeActorBaseData baseData, YeActorRuntimeData runtimeData)
        {
            runtimeData.SetProperty("CurHp", baseData.GetProperty("Hp"));
            runtimeData.SetProperty("CurMp", baseData.GetProperty("Mp"));
        }
    }
}