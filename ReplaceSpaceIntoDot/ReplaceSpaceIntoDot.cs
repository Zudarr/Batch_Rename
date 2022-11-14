using Contract;
using System.Text;

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
            const char Space = ' ';
            const char Dot = '.';
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < originName.Length; i++)
            {
                if (originName[i] == Space)
                {
                    builder.Append(Dot);
                }
                else
                {
                    builder.Append(originName[i]);
                }
                
            }

            var result = builder.ToString();
            return result;
        }
    }
}