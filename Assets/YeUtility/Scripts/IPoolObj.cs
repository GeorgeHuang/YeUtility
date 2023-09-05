namespace CommonUnit
{
    public interface IPoolObj : IReinitializable
    {
        public bool DisposeFlag { get; set; }
    }
}
