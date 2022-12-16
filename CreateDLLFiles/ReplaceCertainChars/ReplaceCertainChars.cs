using Contract;
using System.ComponentModel;
using System.Text;

namespace ReplaceCertainChars
{
    public class ReplaceCertainChars : IRule
    {
        public List<string> CertainChars { get; set; }
        public string Replacement { get; set; }

        public string Name => "ReplaceCertainChars";

        public string Label => "Replace certain chars";
        public string textPreset => $"{Name} CertainChars={String.Join("",CertainChars)},Replace={Replacement}";
        public ReplaceCertainChars()
        {
            CertainChars = new List<string>();
            Replacement = "";
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public object Clone()
        {
            return MemberwiseClone();
        }

        public IRule? Parse(string line)
        {
            var tokens = line.Split(new string[] { " " },
                StringSplitOptions.None);

            var data = tokens[1];
            var attributes = data.Split(new string[] { "," },
                StringSplitOptions.None);

            var pairs1 = attributes[0].Split(new string[] { "=" },
                StringSplitOptions.None); 
            var certains = pairs1[1];
            var pairs2 = attributes[1].Split(new string[] { "=" },
                StringSplitOptions.None);

            var replace = pairs2[1];

            var rule = new ReplaceCertainChars();

            foreach (var c in certains)
            {
                rule.CertainChars.Add($"{c}");
            }

            rule.Replacement = replace;

            return rule;
        }

        public string Rename(string origin)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var c in origin)
            {
                if (CertainChars.Contains($"{c}"))
                {
                    builder.Append(Replacement);
                }
                else
                {
                    builder.Append(c);
                }
            }

            string result = builder.ToString();
            return result;
        }
    }
}