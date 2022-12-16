using Contract;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace ConvertToPascalCase
{
    public class ConvertToPascalCase : IRule
    {
        public string Name => "ConvertToPascalCase";

        public string Label => "Convert to Pascal Case";
        public string textPreset => $"{Name}";
        public event PropertyChangedEventHandler? PropertyChanged;

        public object Clone()
        {
            return MemberwiseClone();
        }

        public IRule? Parse(string data)
        {
            return new ConvertToPascalCase();
        }

        public string Rename(string origin)
        {
            var builder = new StringBuilder();
            string extension = "";
            int lastDotIndex = origin.LastIndexOf('.');

            if (lastDotIndex != -1)
            {
                extension = origin.Substring(origin.LastIndexOf('.'));
            }
            else
            {
                lastDotIndex = origin.Length;
            }

            for (int i = 0; i < lastDotIndex; i++)
            {
                if (!Char.IsLetterOrDigit(origin[i]))
                {
                    builder.Append(" ");
                }
                else
                {
                    builder.Append(origin[i]);
                }
            }

            var result = builder.ToString();
            TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
            result = myTI.ToTitleCase(result).Replace(" ", String.Empty);
            return result + extension;
        }
    }
}