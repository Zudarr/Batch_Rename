using Contract;
using System.Text.RegularExpressions;

namespace ReplaceSpaceIntoDot
{
    public class ReplaceSpaceIntoDot : IRule
    {
        public string Name => "Replace space into dot";
        public string Description => "Reaplce all space into dot";
        public bool IsChecked { get; set; }

        public IRule? Parse(Dictionary<string, string> data)
        {
            if (data["Name"] == Name)
            {
                return new ReplaceSpaceIntoDot();
            }
            return null;
        }

        public string Rename(string originName)
        {
            var result = Regex.Replace(originName, @"\s", ".");
            return result;
        }
    }
}