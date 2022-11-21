using Contract;

namespace AddPrefix
{
    public class AddPrefix : IRule
    {
        public string Name => "Add prefix";
        public string Description => $"Add {Argument}_ to the beginning of the filename";
        public bool IsChecked { get; set; }
        public bool IsRequireArgument => true;
        public string Argument { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public IRule? Parse(Dictionary<string, string> data)
        {
            if (data["Name"] == Name)
            {
                return new AddPrefix()
                {
                    IsChecked = true,
                    Argument = data["Argument"],
                };
            }
            return null;
        }

        public string Rename(string originName)
        {
            return $"{Argument}_{originName}";
        }
    }
}