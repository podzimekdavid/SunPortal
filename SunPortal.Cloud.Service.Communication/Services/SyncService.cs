namespace SunPortal.Cloud.Service.Communication.Services;

public class SyncService: IHostedService
{
    private readonly CommunicationService _communicationService;

    public SyncService(CommunicationService communicationService)
    {
        _communicationService = communicationService;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
       await _communicationService.StartAsync(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _communicationService.StopAsync(cancellationToken);
    }
}