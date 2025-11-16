using demo_158.MVVM.Model;
using demo_158.Repository;
using demo_158.Services.Interfaces;
using System.Collections.ObjectModel;
using demo_158.MVVM.ViewModel;

namespace demo_158.Services
{
    public  class ConversationServices: IConversationServices
    {
        private readonly Func<ConversationViewModel> _createConversationModel;
        private readonly MyInformationRepository _myInformationRepository;
       

        public ConversationServices(Func<ConversationViewModel> createConversationModel, MyInformationRepository myInformationRepository)
        {
            _createConversationModel = createConversationModel;
            _myInformationRepository = myInformationRepository;
        
        }

        public ConversationViewModel ConversationModelMapping(ConversationModelFromServer obj)
        {
            var result = _createConversationModel();

            {
                result.Id = obj.Id;
                result.ContactUserModel = obj.ContactUserModel;
                result.Text = "";
                result.UserModelFromServer = _myInformationRepository.MyUserInfo;
                result.Messages = new ObservableCollection<MessageViewModel>();
            };
            return result;
        }

     
    }
}
