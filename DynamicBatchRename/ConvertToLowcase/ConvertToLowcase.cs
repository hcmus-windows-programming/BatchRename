using Contract;
using System.Text;
using System;

namespace ConvertToLowcase
{
    public class ConvertToLowcase: IRule
    {
        public String Name => "ConvertToLowcase";

        public string Label => "Convert to lowcase";

        public object Clone()
        {
            return MemberwiseClone();
        }

        public IRule? Parse(string data)
        {
            return new ConvertToLowcase();
        }

        public string Rename(string origin)
        {
            var builder = new StringBuilder();
            origin = origin.ToLower();
            for (int i = 0; i < origin.Length; i++)
            {
                if (origin[i] != ' ')
                {
                    builder.Append(origin[i]); 
                }
            }
            var result = builder.ToString();
            return result;
        }
    }
}