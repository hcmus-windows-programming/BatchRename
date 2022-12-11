using Contract;
using System.Text;

namespace ChangeExtension
{
    public class ChangeExtension : IRule
    {
        public String Extension { get; set; }
        public string Name => "ChangeExtension";
        public string Label => "Change extension";

        public object Clone()
        {
            return MemberwiseClone();
        }
        public ChangeExtension()
        {
            Extension = "";
        }
        public IRule? Parse(string line)
        {
            var tokens = line.Split(new string[] { " " },
                StringSplitOptions.None);
            var data = tokens[1];

            var pairs = data.Split(new string[] { "=" },
                StringSplitOptions.None);

            var rule = new ChangeExtension();
            rule.Extension = pairs[1];
            return rule;
        }

        public string Rename(string origin)
        {
            var builder = new StringBuilder();
            int lastIndexDot = origin.LastIndexOf('.');
            builder.Append(origin[0]);

            if(lastIndexDot != -1)
            {
                for(int i = 1; i<lastIndexDot; i++)
                {
                    builder.Append(origin[i]);
                }

                builder.Append(origin[lastIndexDot]);
                for(int i = 0; i<Extension.Length; i++)
                {
                    builder.Append(Extension[i]);
                }
            }
            else
            {
                for (int i = 0; i < origin.Length; i++)
                {
                    builder.Append(origin[i]);
                }
            }

            string result = builder.ToString();
            return result;
        }
    }
}