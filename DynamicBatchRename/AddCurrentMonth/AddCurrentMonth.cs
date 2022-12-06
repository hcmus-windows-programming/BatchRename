using Contract;

namespace AddCurrentMonth
{
    public class AddCurrentMonth : IRule
    {
        public string Name => "AddCurrentMonth";

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