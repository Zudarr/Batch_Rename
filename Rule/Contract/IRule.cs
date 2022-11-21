namespace Contract
{
    public interface IRule: ICloneable
    {
        string Rename(string originName);
        IRule? Parse(Dictionary<string, string> data);
        string Name { get; }
        string Description { get; }
        bool IsChecked { get; set; }
        bool IsRequireArgument { get; }
        public string Argument { get; set; }
    }
}