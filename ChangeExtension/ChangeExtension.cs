using Contract;
using System.Text.RegularExpressions;

namespace ChangeExtension
{
    public class ChangeExtension : IRule
    {
        public string Name => "Change extension";
        public string Description => "Change extension of the filename";
        public bool IsChecked { get; set; }
        public bool IsRequireArgument => true;
        public string Extension { get; set; }

        public IRule? Parse(Dictionary<string, string> data)
        {
            if (data["Name"] == Name)
            {
                return new ChangeExtension()
                {
                    IsChecked = true,
                    Extension = data["Argument"],
                };
            }
            return null;
        }

        public string Rename(string originName)
        {
            var result = Regex.Replace(originName, @"\.[a-z]+$", $".{Extension}");
            return result;
        }
    }
}