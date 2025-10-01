using ModalWindows.Enums;
using ModalWindows.Views.Shared.CustomComponents;

namespace ModalWindows.Services;

public class ModalService
{
    /// <summary>
    /// Событие, на которое подписывается Blazor-компонент.
    /// </summary>
    public event Action<string, NotificationType>? OnShow;

    /// <summary>
    /// Сообщение, которое "ожидает", пока Blazor-компонент будет готов его показать.
    /// </summary>
    public string? PendingMessage { get; private set; }

    /// <summary>
    /// Тип "ожидающего" сообщения.
    /// </summary>
    public NotificationType? PendingType { get; private set; }

    /// <summary>
    /// Метод, который вызывается из MVC-контроллера.
    /// </summary>
    public void Show(string message, NotificationType type)
    {
        // Проверяем, есть ли уже активные подписчики на событие.
        if (OnShow?.GetInvocationList().Any() == true)
        {
            // Если да (например, пользователь кликнул кнопку второй раз, когда
            // Blazor-компонент уже "жив"), то вызываем событие немедленно.
            OnShow.Invoke(message, type);
        }
        else
        {
            // Если нет (самый первый POST-запрос после загрузки страницы),
            // то мы "запоминаем" сообщение в отложенных свойствах.
            PendingMessage = message;
            PendingType = type;
        }
    }

    /// <summary>
    /// Метод для очистки "ожидающего" сообщения после того, как оно было показано.
    /// </summary>
    public void ClearPending()
    {
        PendingMessage = null;
        PendingType = null;
    }
}
