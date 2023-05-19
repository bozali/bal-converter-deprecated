namespace Bal.Converter.Services;

public interface IActivationService
{
    Task ActivateAsync(object activationArgs);
}
