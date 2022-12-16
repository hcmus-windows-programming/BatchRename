using Contract;
using System.ComponentModel;
using System.Text;

namespace AllSpace
{
    public class AllSpace: IRule
    {
        public string Name => "AllSpace";

        public string Label => "Delete all space";
        public string textPreset => $"{Name}";

        public event PropertyChangedEventHandler? PropertyChanged;

        public object Clone()
        {
            return MemberwiseClone();
        }

        public IRule? Parse(string data)
        {
            return new AllSpace();
        }

        public string Rename(string origin)
        {
            var builder = new StringBuilder();
            builder.Append(origin[0]);

            int lastDotIndex = origin.LastIndexOf('.');
            if (lastDotIndex != -1)
            {
                string extension = origin.Substring(lastDotIndex);
                for (int i = 1; i < lastDotIndex; i++)
                {
                    if (origin[i] != ' ')
                    {
                        builder.Append(origin[i]);
                    }
                }
                builder.Append(extension);
            }
            else
            {
                for (int i = 1; i < origin.Length; i++)
                {
                    if (origin[i] != ' ')
                    {
                        builder.Append(origin[i]);
                    }
                }
            }

            var result = builder.ToString();
            return result;
        }
    }
}