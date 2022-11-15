using Contract;
using System.Globalization;

namespace PascalCase
{
    public class PascalCase : IRule
    {
        public string Name => "Pascal case";

        public string Description => "Change file name into PascalCase";

        public bool IsChecked { get; set; }

        public IRule? Parse(Dictionary<string, string> data)
        {
            if (data["Name"] == Name)
            {
                return new PascalCase();
            }
            return null;
        }

        public string Rename(string originName)
        {
            TextInfo info = CultureInfo.CurrentCulture.TextInfo;
            var result = info.ToTitleCase(originName);

            return result;
        }
    }
}