using Contract;

namespace AddCounter
{
    public class AddCounter : IRule
    {
        public string Name => "Add counter";
        public string Description => "Add counter to the end of file";
        public bool IsChecked { get; set; }
        public bool IsRequireArgument => true;
        public List<string> Argument { get; set; }

        public AddCounter()
        {
            Argument = new List<string>();
        }

        public IRule? Parse(Dictionary<string, string> data)
        {
            if (data["Name"] == Name)
            {
                return new AddCounter();
            }
            return null;
        }

        public string Rename(string originName)
        {
            throw new NotImplementedException();
        }
    }
}