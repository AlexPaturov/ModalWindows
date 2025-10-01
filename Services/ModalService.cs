using ModalWindows.Views.Shared.CustomComponents;

namespace ModalWindows.Services;

public class ModalService
{
    // Событие, которое будет "сообщать" компоненту, что его нужно показать
    public event Action<string, NotificationModal.NotificationType>? OnShow;

    // Метод, который будет вызывать контроллер
    public void Show(string message, NotificationModal.NotificationType type = NotificationModal.NotificationType.Success)
    {
        OnShow?.Invoke(message, type);
    }
}
