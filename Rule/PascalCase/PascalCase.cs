using Contract;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace PascalCase
{
    public class PascalCase : IRule
    {
        public string Name => "Pascal case";
        public string Description => "Change file name into PascalCase";
        public bool IsChecked { get; set; }
        public bool IsRequireArgument => false;
        public string Argument { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public IRule? Parse(Dictionary<string, string> data)
        {
            if (data["Name"] == Name)
            {
                return new PascalCase()
                {
                    IsChecked = true,
                    Argument = "",
                };
            }
            return null;
        }

        public string Rename(string originName)
        {
            StringBuilder builder = new StringBuilder();

            //cắt bỏ phần extension của file
            var temp = originName.Remove(originName.LastIndexOf('.'));

            //PascalCase phần còn lại
            TextInfo info = CultureInfo.CurrentCulture.TextInfo;
            temp = info.ToTitleCase(temp);
            builder.Append(temp);

            //nối extension vào phần đã PascalCase
            Regex pattern = new Regex(@"\.[a-z]+$");
            var match = pattern.Match(originName);
            builder.Append(match.ToString());

            var result = builder.ToString();
            return result;
        }
    }
}