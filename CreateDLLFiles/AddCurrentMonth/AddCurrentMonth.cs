using Contract;
using System.ComponentModel;

namespace AddCurrentMonth
{
    public class AddCurrentMonth : IRule
    {
        public string Name => "AddCurrentMonth";

        public string Label => "Add current month";

        public event PropertyChangedEventHandler? PropertyChanged;
        public string textPreset => $"{Name}";
        public object Clone()
        {
            return MemberwiseClone();
        }

        public IRule? Parse(string data)
        {

            return new AddCurrentMonth();
        }

        public string Rename(string origin)
        {
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year % 100 ;

            string result = $"{year}{month} {origin}";
            return result;
        }
    }
}