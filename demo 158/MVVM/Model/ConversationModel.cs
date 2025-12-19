namespace demo_158.MVVM.Model;


public class ConversationModel
{
    public int Id { get; set; }
    public bool IsConversationPrivateChat { get; set; }
    public DateTime CreatedTime { get; set; }
    public ContactUserModel ContactUserModel { get; set; }

}