using demo_158.MVVM.Model;
using demo_158.Repository;
using demo_158.Services.Interfaces;
using System.Collections.ObjectModel;
namespace demo_158.Services
{
    public  class ConversationServices: IConversationServices
    {
        private readonly Func<ConversationModel> _createConversationModel;
        private readonly MyInformationRepository _myInformationRepository;
       

        public ConversationServices(Func<ConversationModel> createConversationModel, MyInformationRepository myInformationRepository)
        {
            _createConversationModel = createConversationModel;
            _myInformationRepository = myInformationRepository;
        
        }

        public ConversationModel ConversationModelMapping(ConversationModelFromServer obj)
        {
            var result = _createConversationModel();

            {
                result.Id = obj.Id;
                result.ContactUserInfo = obj.ContactUserInfo;
                result.Text = "";
                result.UserModelFromServer = _myInformationRepository.MyUserInfo;
                result.Messages = new ObservableCollection<MessagesModel>();
            };
            return result;
        }

     
    }
}
