using Contract;
using System.Text.RegularExpressions;

namespace AddSuffix
{
    public class AddSuffix : IRule
    {
        public string Name => "Add suffix";
        public string Description => $"Add _{Argument} to the end of the filename";
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
                return new AddSuffix()
                {
                    IsChecked = true,
                    Argument = data["Argument"],
                };
            }
            return null;
        }

        public string Rename(string originName)
        {
            Regex pattern = new Regex(@"\.[a-z]+$");
            var match = pattern.Match(originName);

            var result = Regex.Replace(originName, @"\.[a-z]+$", $"_{Argument}{match}");
            return result;
        }
    }
}