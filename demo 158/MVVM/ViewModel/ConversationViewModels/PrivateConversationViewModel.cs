using demo_158.Base;
using demo_158.Hubs;
using demo_158.MVVM.Model;
using demo_158.Services.Interfaces;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Messaging;
using demo_158.EventsPublish;
using demo_158.Services.Enums;

namespace demo_158.MVVM.ViewModel.ConversationViewModels;

public class PrivateConversationViewModel : ConversationViewModel
{
    private readonly ConnectionManager _connection;
    private readonly IMessageServices _messagesServices;
    private readonly IServiceProvider _service;
    private ContactUserModel _contactUserModel;
    private ICommand _deleteConversation;
    public ContactUserModel ContactUserModel
    {
        get => _contactUserModel;
        set => SetField(ref _contactUserModel, value);
    }
    public PrivateConversationViewModel(ConnectionManager connection, IMessageServices messagesServices, IServiceProvider service) : base(connection, messagesServices, service)
    {
        _connection = connection;
        _messagesServices = messagesServices;
        _service = service;
    }

    public ICommand DeleteConversation => _deleteConversation ?? new GeneralCommand(async () =>
    {
        var result = await _connection.InvokeAsync("DeleteConversation", Id, ContactUserModel.ContactUsername);

        if (result is ServerAnswer.ok)
        {
            WeakReferenceMessenger.Default.Send(new SuccessDeletedConversation(Id));
        }
    });

}