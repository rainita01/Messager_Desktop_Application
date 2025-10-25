using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Server_API_With_SignalR_For_Messager_01.Models;

namespace demo_158.Services
{
    public class LastMessageValueConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is LastMessageModel lastMessage)
            {
                if (!string.IsNullOrWhiteSpace(lastMessage.Text))
                    return lastMessage.Text;

                return lastMessage.MessageType;
            }

            return "";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        
    }
}
