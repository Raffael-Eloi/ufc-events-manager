namespace UFC.Events.Manager.API.Features.Emails.ValidateEmail;

public interface IValidateEmail
{
    bool Execute(string email);
}
