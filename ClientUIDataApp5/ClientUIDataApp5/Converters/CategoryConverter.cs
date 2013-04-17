using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using ClientUIDataApp5.ModelServices;
using ClientUIDataApp5.DomainModel;

namespace ClientUIDataApp5.Converters
{
    public class CategoryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            CategoriesRepository repository = CategoriesRepository.Instance as CategoriesRepository;

            if (repository != null && value != null)
            {
                int categoryID = (int)value;
                Category category = repository.EntitySet.Where(c => c.CategoryID == categoryID).FirstOrDefault();

                if (category != null)
                    return category.CategoryName;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


}
