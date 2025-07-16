using MVCWebApp.BackgroundServices;

namespace MVCWebApp.Services
{
    public interface ITaskQueueService
    {
        Task AddSendEmailQueue(string recipient, string subject, string body);
    }

    public class TaskQueueService(
        IBackgroundTaskQueue taskQueue,
        IEmailService _emailService) : ITaskQueueService
    {
        public async Task AddSendEmailQueue(string recipient, string subject, string body)
        {
            taskQueue.QueueBackgroundWorkItem(async token =>
            {
                await _emailService.SendEmailAsync(
                    recipient,
                    subject,
                    body);
            });
        }
    }
}
