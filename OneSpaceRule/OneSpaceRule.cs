using Contract;
using System.Text;

namespace OneSpaceRule
{
    public class OneSpaceRule : IRule
    {
        public string name => "OneSpace";

        public IRule? Parse(string data)
        {
            return new OneSpaceRule();
        }

        public string Rename(string originName)
        {
            const char space = ' ';
            StringBuilder builder= new StringBuilder();

            for (int i = 1; i < originName.Length; i++)
            {
                char currentChar = originName[i];
                char previousChar = originName[i - 1];
                if (currentChar == space && previousChar == space)
                {
                    //do nothing
                }
                else
                {
                    builder.Append(currentChar);
                }
            }

            string result = builder.ToString();
            return result;
        }
    }
}