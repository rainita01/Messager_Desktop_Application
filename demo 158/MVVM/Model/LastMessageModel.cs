using demo_158.MVVM.Model;

namespace Server_API_With_SignalR_For_Messager_01.Models
{
    public class LastMessageModel
    {
        public DateTime SendDate { get; set; }
        public string? Text { get; set; }
        public MessageTypes MessageType { get; set; }
    }
}
