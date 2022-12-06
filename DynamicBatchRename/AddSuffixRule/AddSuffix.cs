using System.Text;
using Contract;

namespace AddSuffixRule
{
    public class AddSuffix : IRule
    {
            public string Suffix { get; set; }

            public string Name => "AddSuffix";

            public AddSuffix()
            {
                Suffix = "";
            }

            public string Rename(string origin)
            {
                var builder = new StringBuilder();
                int lastDotIndex = origin.LastIndexOf('.');

            if (lastDotIndex != -1)
            {
                builder.Append(origin.Substring(0, lastDotIndex));
                builder.Append(" ");
                builder.Append(Suffix);
                builder.Append(origin.Substring(lastDotIndex));
            }
            else
            {
                builder.Append(origin);
                builder.Append(" ");
                builder.Append(Suffix);
            }
            string result = builder.ToString();
                return result;
            }

            public object Clone()
            {
                return MemberwiseClone();
            }

            public IRule Parse(string line)
            {
                var tokens = line.Split(new string[] { " " },
                    StringSplitOptions.None);
                var data = tokens[1];

                var pairs = data.Split(new string[] { "=" },
                    StringSplitOptions.None);

                var rule = new AddSuffix();
                rule.Suffix = pairs[1];
                return rule;
            }
    }
}