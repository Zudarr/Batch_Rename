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
        public List<string> Argument { get; set; }

        public RemoveSpecialCharacters()
        {
            Argument = new List<string>();
        }

        public IRule? Parse(Dictionary<string, string> data)
        {
            if (data["Name"] == Name)
            {
                var tokens = data["Argument"].Split(", ", StringSplitOptions.None);
                var result = new RemoveSpecialCharacters()
                {
                    IsChecked = true,
                };

                foreach (var token in tokens)
                {
                    result.Argument.Add(token);
                }

                return result;
            }
            return null;
        }

        public string Rename(string originName)
        {
            StringBuilder builder = new StringBuilder();

            foreach (var character in originName)
            {
                if (Argument.Contains(character.ToString()))
                {
                    builder.Append(' ');
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