using back_end_cemex.Entities;
using back_end_cemex.Helpers.Email;
using back_end_cemex.Helpers.Email.ForgotPassword;

namespace back_end_cemex.Services
{
    public interface IEmailService
    {
        bool SendEmail(EmailData emailData);
        bool SendEmailWithAttachment(EmailDataWithAttachment emailData);
        bool SendUserEmail(UserForgotPassword userForgotPassword);
    }
}
