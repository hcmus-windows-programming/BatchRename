using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Contract;

namespace BatchRename
{
	public class PreviewRenameConverter : IValueConverter
	{
		public List<IRule> Rules { get; set; }

		public PreviewRenameConverter()
		{
			Rules = new List<IRule>();
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string origin = (string)value;

			string NewName = origin;

			foreach (var rule in Rules)
			{
				NewName = rule.Rename(NewName);
			}
			return NewName;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
            string NewName = (string)value;
            return NewName;
        }
	}
}
