using Contract;
using System.Text.RegularExpressions;

namespace ChangeExtension
{
    public class ChangeExtension : IRule
    {
        public string Name => "Change extension";
        public string Description => $"Change extension of the filename to .{Argument}";
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
                return new ChangeExtension()
                {
                    IsChecked = true,
                    Argument = data["Argument"],
                };
            }
            return null;
        }

        public string Rename(string originName)
        {   //regex khớp với extension của file (đuôi file)
            var result = Regex.Replace(originName, @"\.[a-z]+$", $".{Argument}");
            return result;
        }
    }
}