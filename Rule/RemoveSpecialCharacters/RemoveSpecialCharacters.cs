using Contract;
using System.Text;

namespace RemoveSpecialCharacters
{
    public class RemoveSpecialCharacters: IRule
    {
        public string Name => "Remove special character";
        public string Description => "Remove some special character given in advance";
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
                var result = new RemoveSpecialCharacters()
                {
                    IsChecked = true,
                    Argument = data["Argument"]
                };

                return result;
            }
            return null;
        }

        public string Rename(string originName)
        {
            StringBuilder builder = new StringBuilder();

            var SpecialChar = Argument.Split(", ", StringSplitOptions.None);

            foreach (var character in originName)
            {
                if (SpecialChar.Contains(character.ToString()))
                {
                    builder.Append(string.Empty);
                }
                else
                {
                    builder.Append(character);
                }
            }

            var result = builder.ToString();
            return result;
        }
    }
}