using UFC.Events.Manager.API.Features.Emails.ValidateEmail;

namespace UFC.Events.Manager.API.Tests.Features.Emails;

internal class ValidateEmailTest
{
    private IValidateEmail _validateEmail;

    [SetUp]
    public void Setup()
    {
        _validateEmail = new ValidateEmail();
    }

    [Test]
    public void GivenEmptyEmail_WhenValidating_ThenItShouldBeInvalid()
    {
        // Arrange
        string email = string.Empty;

        // Act
        bool isValid = _validateEmail.Execute(email);

        // Assert
        Assert.That(isValid, Is.False);
    }

    [Test]
    public void GivenEmailWithoutAtSign_WhenValidating_ThenItShouldBeInvalid()
    {
        // Arrange
        string email = "johndoe.com";

        // Act
        bool isValid = _validateEmail.Execute(email);

        // Assert
        Assert.That(isValid, Is.False);
    }

    [Test]
    public void GivenEmailWithDisplayName_WhenValidating_ThenItShouldBeInvalid()
    {
        // Arrange
        string email = "John Doe <john@doe.com>";

        // Act
        bool isValid = _validateEmail.Execute(email);

        // Assert
        Assert.That(isValid, Is.False);
    }

    [Test]
    public void GivenWellFormedEmail_WhenValidating_ThenItShouldBeValid()
    {
        // Arrange
        string email = "john@doe.com";

        // Act
        bool isValid = _validateEmail.Execute(email);

        // Assert
        Assert.That(isValid, Is.True);
    }
}
