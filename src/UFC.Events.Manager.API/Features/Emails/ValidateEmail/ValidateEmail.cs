using System.Net.Mail;

namespace UFC.Events.Manager.API.Features.Emails.ValidateEmail;

public sealed class ValidateEmail : IValidateEmail
{
    public bool Execute(string email)
    {
        return MailAddress.TryCreate(email, out MailAddress? address)
            && address.Address == email;
    }
}
