using Microsoft.AspNetCore.Http;

namespace back_end_cemex.Helpers.Email
{
    public class EmailDataWithAttachment : EmailData
    {
        public IFormFileCollection EmailAttachments { get; set; }
    }
}
