namespace Contract
{
    public interface IRule : ICloneable
    {
        string Rename(string origin);
        IRule? Parse(string data);
        string Name { get; }
    }
}