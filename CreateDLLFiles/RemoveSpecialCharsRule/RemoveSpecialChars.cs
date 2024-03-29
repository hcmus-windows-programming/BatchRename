﻿using Contract;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace RemoveSpecialCharsRule
{
    public class RemoveSpecialCharsRule : IRule
    {
        public List<string> SpecialChars { get; set; }
        public string Replacement { get; set; }

        public string Name => "RemoveSpecialChars";

        public string Label => "Remove special chars";
        public string textPreset => $"{Name} SpecialChars={String.Join("",SpecialChars)}";

        public RemoveSpecialCharsRule()
        {
            SpecialChars = new List<string>();
            Replacement = " ";
        } 

        public event PropertyChangedEventHandler? PropertyChanged;

        public string Rename(string origin)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var c in origin)
            {
                if (SpecialChars.Contains($"{c}"))
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

        public object Clone()
        {
            return MemberwiseClone();
        }

        public IRule Parse(string line)
        {
            var tokens = line.Split(new string[] { " " },
                StringSplitOptions.None);
            var data = tokens[1]; // SpecialChars=-_
            var pairs = data.Split(new string[] { "=" },
                StringSplitOptions.None); // -_
            var specials = pairs[1];

            var rule = new RemoveSpecialCharsRule();

            foreach (var c in specials)
            {
                rule.SpecialChars.Add($"{c}");
            }

            return rule;
        }
    }

}