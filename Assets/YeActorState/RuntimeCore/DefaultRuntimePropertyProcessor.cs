namespace YeActorState.RuntimeCore
{
    public class DefaultRuntimePropertyProcessor: IBasePropertyProcessor
    {
        public void Processor(YeActorBaseData baseData, YeActorRuntimeData runtimeData)
        {
            var moveSpeed = runtimeData.GetProperty("MoveSpeed");
            var moveSpeedRatio = runtimeData.GetProperty("MoveSpeedRatio");
            moveSpeed *= (1 + moveSpeedRatio * 0.01f);
            runtimeData.SetProperty("MoveSpeed", moveSpeed);
        }
    }
}