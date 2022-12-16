using Contract;
using System.ComponentModel;
using System.Text;

namespace AddCounterRule
{
    public class AddCounterRule : IRule
    {
        private int _current = 0;
        private int _start = 0;

        public event PropertyChangedEventHandler? PropertyChanged;

        public int Start
        {
            get => _start; set
            {
                _start = value;

                _current = value;
            }
        }
        public int Step { get; set; }
        public int numDigits { get; set; }
        
        public string Name => "AddCounter";

        public string Label => "Add counter";

        public string textPreset => $"{Name} Start={Start},Step={Step},numDigits={numDigits}";
        public AddCounterRule()
        {
            Start = 1;
            Step = 2;
            numDigits = 0; //No set number of digits
        }
        public string Rename(string origin)
        {
            var builder = new StringBuilder();
            int lastDotIndex = origin.LastIndexOf('.');

            if (lastDotIndex != -1)
            {
                builder.Append(origin.Substring(0, lastDotIndex));
                builder.Append(" ");
                builder.Append(_current.ToString($"D{numDigits}"));
                builder.Append(origin.Substring(lastDotIndex));
            }
            else
            {
                builder.Append(origin);
                builder.Append(" ");
                builder.Append(_current.ToString($"D{numDigits}"));
            }

            _current += Step;

            string result = builder.ToString();
            return result;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public IRule Parse(string line)
        {
            var rule = new AddCounterRule();

            var tokens = line.Split(new string[] { " " },
                StringSplitOptions.None);
            var data = tokens[1];
            var attributes = data.Split(new string[] { "," },
                StringSplitOptions.None);

            var pairs0 = attributes[0].Split(new string[] { "=" },
                StringSplitOptions.None);
            rule.Start = int.Parse(pairs0[1]);

            var pairs1 = attributes[1].Split(new string[] { "=" },
                StringSplitOptions.None);
            rule.Step = int.Parse(pairs1[1]);

            if (attributes.Length == 3)
            {
                var pairs2 = attributes[2].Split(new string[] { "=" },
                StringSplitOptions.None);
                rule.numDigits = int.Parse(pairs2[1]);
            }
            return rule;
        }
    }
}