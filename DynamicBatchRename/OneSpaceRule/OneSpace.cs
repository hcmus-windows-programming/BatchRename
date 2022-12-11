using Contract;
using System.Text;

namespace OneSpaceRule
{
    public class OneSpace : IRule
    {
        public string Name => "OneSpace";

        public string Label => "One space";

        public object Clone()
        {
            return MemberwiseClone();
        }

        public IRule? Parse(string data)
        {
            return new OneSpace();
        }

        public string Rename(string origin)
        {
            var builder = new StringBuilder();
            builder.Append(origin[0]);

            for (int i = 1; i < origin.Length; i++)
            {
                if (origin[i] == ' ')
                {
                    if (origin[i - 1] != ' ')
                    {
                        builder.Append(origin[i]);
                    }
                }
                else
                {
                    builder.Append(origin[i]);
                }
            }

            var result = builder.ToString();
            return result;
        }
    }
}