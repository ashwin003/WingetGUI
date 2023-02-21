using System.Threading.Tasks;

namespace WingetGUI.App.Activation
{
    public interface IActivationHandler
    {
        bool CanHandle(object args);

        Task HandleAsync(object args);
    }
}