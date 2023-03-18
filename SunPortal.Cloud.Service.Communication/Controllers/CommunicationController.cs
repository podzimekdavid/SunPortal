using Microsoft.AspNetCore.Mvc;
using SunPortal.Cloud.Service.Communication.Hub;
using SunPortal.Communication.Packets;

namespace SunPortal.Cloud.Service.Communication.Controllers;

public class CommunicationController : Controller
{
    /*private readonly CommunicationHub _hub;

    public CommunicationController(CommunicationHub hub)
    {
        _hub = hub;
    }

    [HttpGet("/test")]
    public async Task<IActionResult> Test(int parameterId, int address, Guid clientId)
    {
        return Json(await _hub.Request(new()
        {
            ParameterId = parameterId,
            Address = address,
            RequestId = Guid.NewGuid()
        }, clientId));
    }*/
}