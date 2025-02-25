using BusinessLayer.Helper;
using BusinessLayer.Interface;

namespace BusinessLayer.Repository;

public class EmailService : IEmailService
{
    public async Task<bool> SendEmailAsync(string email, string subject, string message)
        {
            return await EmailHelper.SendEmailAsync(email, subject, message);
        }
}
