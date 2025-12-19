using demo_158.Hubs;
using demo_158.Services.Interfaces;

namespace demo_158.MVVM.ViewModel.ConversationViewModels;

public class PrivateConversationViewModel : ConversationViewModel
{
    private readonly ConnectionManager _connection;
    private readonly IMessageServices _messagesServices;
    private readonly IServiceProvider _service;

    public PrivateConversationViewModel(ConnectionManager connection, IMessageServices messagesServices, IServiceProvider service) : base(connection, messagesServices, service)
    {
        _connection = connection;
        _messagesServices = messagesServices;
        _service = service;
    }
}