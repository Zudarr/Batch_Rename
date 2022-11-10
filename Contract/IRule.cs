namespace Contract
{
    public interface IRule
    {
        string Rename(string originName);
        IRule? Parse(string data);
        string name { get; }
    }
}