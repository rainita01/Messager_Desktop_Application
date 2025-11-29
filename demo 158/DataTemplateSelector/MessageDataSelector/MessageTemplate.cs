using System.Windows;
using demo_158.MVVM.ViewModel;
using Microsoft.Identity.Client;

namespace demo_158.DataTemplateSelector.MessageDataSelector;

public class MessageTemplate : System.Windows.Controls.DataTemplateSelector
{
    public DataTemplate MyTextMessageTemplate { get; set; }


    public DataTemplate OtherTextMessageTemplate { get; set; }  


    public override DataTemplate? SelectTemplate(object? item, DependencyObject container)
    {
        var message = item as MessageViewModel;

        if (message.Message.IsMyMessage)
        {
            return MyTextMessageTemplate;
        }
        else
        {
            return OtherTextMessageTemplate;
        }
    
    }
}