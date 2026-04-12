namespace DCA_Padel_Club.Core.Application.AppEntry;

public class ServiceNotFoundException : Exception
{
    public ServiceNotFoundException(string serviceName)
        : base($"No service of type '{serviceName}' has been registered.")
    {
    }
}
