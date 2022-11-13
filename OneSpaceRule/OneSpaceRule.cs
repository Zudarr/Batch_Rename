using Contract;
using System.Text;

namespace OneSpaceRule
{
    public class OneSpaceRule : IRule
    {
        public string Name => "One space rule";
        public string Description => "Remove contiguous spaces, leaving only one";
        public bool IsChecked { get; set; }

        public IRule? Parse(string data)
        {
            if (data == Name)
            {
                return new OneSpaceRule();
            }
            return null;
        }

        public string Rename(string originName)
        {
            const char space = ' ';
            StringBuilder builder = new StringBuilder();

            builder.Append(originName[0]);
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