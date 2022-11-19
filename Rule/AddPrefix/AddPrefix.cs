using Contract;

namespace AddPrefix
{
    public class AddPrefix : IRule
    {
        public string Name => "Add prefix";
        public string Description => $"Add {Prefix}_ to the beginning of the filename";
        public bool IsChecked { get; set; }
        public bool IsRequireArgument => true;
        public string Prefix { get; set; }

        public IRule? Parse(Dictionary<string, string> data)
        {
            if (data["Name"] == Name)
            {
                return new AddPrefix()
                {
                    IsChecked = true,
                    Prefix = data["Argument"],
                };
            }
            return null;
        }

        public string Rename(string originName)
        {
            return $"{Prefix}_{originName}";
        }
    }
}