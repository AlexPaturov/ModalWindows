using ModalWindows.Enums;
using ModalWindows.Views.Shared.CustomComponents;

namespace ModalWindows.Services;

public class ModalService
{
    public event Action<string, NotificationType>? OnShow;

    // Этот метод будет вызываться только Blazor-компонентом
    public void Show(string message, NotificationType type)
    {
        OnShow?.Invoke(message, type);
    }
}
