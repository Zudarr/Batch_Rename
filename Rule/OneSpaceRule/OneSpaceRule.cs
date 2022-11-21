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
        public string Argument { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public IRule? Parse(Dictionary<string, string> data)
        {
            if (data["Name"] == Name)
            {
                return new OneSpaceRule()
                {
                    IsChecked = true,
                    Argument = "",
                };
            }
            return null;
        }

        public string Rename(string originName)
        {
            var result = Regex.Replace(originName, @"\s+", " "); // regex khớp với từ 2 khoảng trắng liên tiếp trở lên
            return result;
        }
    }
}