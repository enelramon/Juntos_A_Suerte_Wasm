using Radzen;

namespace Juntos_A_Suerte_Wasm.Extensors
{
    public static class Notificacions
    {
        public static void ShowNotification(this NotificationService notifier,
            string title,
            string message,
            NotificationSeverity severity)
        {
            var sent = new NotificationMessage
            {
                Severity = severity,
                Summary = title,
                Detail = message,
                Duration = 3000
            };

            notifier.Notify(sent);
        }

    }
}