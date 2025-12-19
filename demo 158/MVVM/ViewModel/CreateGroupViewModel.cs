using System.Windows.Input;
using demo_158.Base;

namespace demo_158.MVVM.ViewModel;

public class CreateGroupViewModel : ViewModelBase
{
    private ICommand _coloseWindowCommand;
    private ICommand _createGroupCommand;
    public event EventHandler RequestClose;

    public CreateGroupViewModel()
    {
        
    }

    public ICommand ClosWindowCommand => _coloseWindowCommand ??
                                         new GeneralCommand(() => { RequestClose.Invoke(this, EventArgs.Empty); });
    public ICommand CreateGroupCommand => _createGroupCommand ?? new GeneralCommand(CreateGroupExecute);

    private void CreateGroupExecute()
    {
        
    }
}