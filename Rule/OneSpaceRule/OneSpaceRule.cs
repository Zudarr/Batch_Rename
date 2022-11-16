using Contract;
using System.Text.RegularExpressions;

namespace OneSpaceRule
{
    public class OneSpaceRule : IRule
    {
        public string Name => "One space";
        public string Description => "Remove contiguous spaces, leaving only one";
        public bool IsChecked { get; set; }
        public bool IsRequireArgument => false;

        public IRule? Parse(Dictionary<string, string> data)
        {
            if (data["Name"] == Name)
            {
                return new OneSpaceRule()
                {
                    IsChecked = true,
                };
            }
            return null;
        }

        public string Rename(string originName)
        {
            var result = Regex.Replace(originName, @"\s+", " ");
            return result;
        }
    }
}