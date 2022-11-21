using Contract;
using System.Text.RegularExpressions;

namespace AddCounter
{
    public class AddCounter : IRule
    {
        public string Name => "Add counter";
        public string Description => "Add counter to the end of file";
        public bool IsChecked { get; set; }
        public bool IsRequireArgument => false;
        public string Argument { get; set; }
        public int Counter { get; set; }


        public IRule? Parse(Dictionary<string, string> data)
        {
            if (data["Name"] == Name)
            {
                return new AddCounter()
                {
                    IsChecked = true,
                    Argument = "",
                    Counter = 1,
                };
            }
            return null;
        }

        public string Rename(string originName)
        {
            Regex pattern = new Regex(@"\.[a-z]+$");
            var match = pattern.Match(originName);

            var result = Regex.Replace(originName, @"\.[a-z]+$", $"_{Counter}{match}");
            Counter++;
            return result;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}